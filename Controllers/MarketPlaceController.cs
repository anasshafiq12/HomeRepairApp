using HouseRepairApp.Data;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseRepairApp.Controllers
{
	public class MarketPlaceController : Controller
	{
        private readonly ApplicationDbContext _context;
		public MarketPlaceController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Index(CartItem item)
		{
			//_context.CartItems.Add(item); 

			return View();
		}
		public IActionResult Cart()
		{
			List<CartItem> items = _context.CartItems.ToList();
			// store them in session
			Cart cart = new Cart { CartItems = items };
			cart.SetTotalPrice();
			return View(items);
		}
		[HttpPost]
		public IActionResult Cart(int id,int quantity)
		{
            // stored items in session update their price
            List<CartItem> items = _context.CartItems.ToList();// get from session

            Cart cart = new Cart { CartItems = items };
			cart.SetItemPrice(id,quantity);
            cart.SetTotalPrice();
            return View(items);
        }
	}
}
