using HouseRepairApp.Data;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseRepairApp.Controllers
{
    public class AdminController : Controller
	{
        private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _context;
        private readonly SignInManager<MyUser> _signInManager;
        private readonly UserManager<MyUser> _userManager;
        public AdminController(IWebHostEnvironment env, ApplicationDbContext context, SignInManager<MyUser> signInManager, UserManager<MyUser> userManager)
        {
            _context = context;
			_environment = env;
            _signInManager = signInManager;
            _userManager = userManager;
        }
		
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Home()
		{
			return View();
		}
        public IActionResult AddItem()
		{
			return View();

		}
		[HttpPost]
		public IActionResult AddItem(CartItem item,IFormFile image)
		{
			if (item == null || image == null)
			{
				ModelState.AddModelError(string.Empty, "Invalid input data. Please ensure all fields are filled.");
				return View(item);
			}
			try
			{
                string wwwPath = _environment.WebRootPath;
                string uploadsPath = Path.Combine(wwwPath, "uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Generate unique file name to avoid overwrites
                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(uploadsPath, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                // Save image URL or relative path to the item
                item.ImageUrl = Path.Combine("uploads", fileName);
                _context.CartItems.Add(item);
				_context.SaveChanges();
				return View();
			}
			catch (Exception ex)
			{
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request: " + ex.Message);
                return View(item);
            }
		}
		public IActionResult Bookings()
		{
			List<Booking> bookings = _context.Bookings.ToList();
			return View(bookings);
		}
		[HttpPost]
		public IActionResult Delete(int id)
		{
            Booking booking = _context.Bookings.FirstOrDefault(z => z.Id == id); 
			_context.Bookings.Remove(booking);
            _context.SaveChanges();
            return RedirectToAction("Bookings", "Admin");
		}
		public IActionResult Users()
		{
            List<MyUser> users = _context.Users.ToList();
            return View(users);
		}
		public List<Cart> GetCart(string userId)
		{
			List<Cart> cart = _context.Carts.Where(u => u.UserId == userId).ToList();
			return cart;
		}
		public IActionResult OrdersList()
		{
			List<MyUser> users = _context.Users.ToList();
			// List<Cart> carts = new List<Cart>();
			List<Order> orders = new List<Order>();
			foreach (MyUser user in users)
			{
				List<Cart> carts = GetCart(user.Id);
				foreach (Cart cart in carts)
				{
					List<SelectedCartItem> selectedItems = _context.SelectedCartItems.Where(c => c.CartId == cart.CartId).ToList();
					cart.SelectedCartItems = selectedItems;
					Order order = new Order { Cart = cart, cartItems = selectedItems };
					order.User = user;
					orders.Add(order);
				}
			}
			return View(orders);
		}
		[HttpPost]
		public IActionResult DeleteOrder(int cartId)
		{
			Cart cart = _context.Carts.FirstOrDefault(c => c.CartId == cartId);
			if(cart != null)
				_context.Carts.Remove(cart);
			List<SelectedCartItem> selectedCartItems = _context.SelectedCartItems.Where(c => c.CartId == cartId).ToList();
			foreach(var item in selectedCartItems)
			{
				if(item != null)
					_context.SelectedCartItems.Remove(item);
			}
			_context.SaveChanges();
			return RedirectToAction("OrdersList","Admin");
		}

		public IActionResult ChatWithCustomer()
		{
			return View();
		}
    }
}
