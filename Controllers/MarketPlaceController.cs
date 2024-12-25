using HouseRepairApp.Data;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text.Json;

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
			List<CartItem> items = _context.CartItems.ToList();
            string json = JsonSerializer.Serialize(items);
			HttpContext.Session.SetString("cartItems", json);
			return View(items);
		}
        [HttpPost]
        public IActionResult Index(string id)
        {
            if (!HttpContext.Session.Keys.Contains("ids"))
            {
                List<string> ids = new List<string> { id };
                string jsonIds = JsonSerializer.Serialize(ids);
                HttpContext.Session.SetString("ids", jsonIds);
            }
            else
            {
                string json = HttpContext.Session.GetString("ids");
                List<string> ids = JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
                if (!ids.Contains(id))
                {
                    ids.Add(id);
                    string jsonIds = JsonSerializer.Serialize(ids);
                    HttpContext.Session.SetString("ids", jsonIds);
                }
            }

            string jsonItems = HttpContext.Session.GetString("cartItems");
            List<CartItem> items = JsonSerializer.Deserialize<List<CartItem>>(jsonItems) ?? new List<CartItem>();
            return View(items);
        }
        public IActionResult Cart()
        {
            // If items already selected
            if(HttpContext.Session.Keys.Contains("items"))
            {
                string jsonitems = HttpContext.Session.GetString("items");
                List<CartItem> cartItems = JsonSerializer.Deserialize<List<CartItem>>(jsonitems) ?? new List<CartItem>();
                Cart cartP = new Cart { CartItems = cartItems };
                cartP.SetTotalPrice();
                if(HttpContext.Session.Keys.Contains("orderStatus"))
                    ViewBag.Status = "Order Placed"; // Pass TempData to the view via ViewBag
                return View(cartP);
            }
            // if items not already selected
            string json = HttpContext.Session.GetString("ids");
            if (string.IsNullOrEmpty(json))
                return RedirectToAction("Index", "MarketPlace");
            List<string> ids = JsonSerializer.Deserialize<List<string>>(json);
            List<CartItem> items = new List<CartItem>();
            foreach (string id in ids)
            {
                CartItem item = _context.CartItems.FirstOrDefault(z => z.ItemId == int.Parse(id));
                items.Add(item);
            }
            string jsonItems = JsonSerializer.Serialize(items);
            HttpContext.Session.SetString("items", jsonItems);
            Cart cart = new Cart { CartItems = items };
            cart.SetTotalPrice();
			ViewBag.Status = TempData["Status"]; // Pass TempData to the view via ViewBag
			return View(cart);
        }
        [HttpPost]
        public IActionResult Cart(int id, int quantity)
        {
            string json = HttpContext.Session.GetString("items");
            List<CartItem> items = JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
            Cart cart = new Cart { CartItems = items };
            if (quantity == 0)
            {
                items.RemoveAll(z => z.ItemId == id);
                string jsonIds = HttpContext.Session.GetString("ids");
                List<string> ids = JsonSerializer.Deserialize<List<string>>(jsonIds);
                string userId = id.ToString();
                if (ids.Remove(userId))
                {
                    jsonIds = JsonSerializer.Serialize(ids);
                    HttpContext.Session.SetString("ids", jsonIds);
                }
            }
            cart.SetItemPriceAndQuantity(id, quantity);
            cart.SetTotalPrice();

            string jsonItems = JsonSerializer.Serialize(cart.CartItems);
            HttpContext.Session.SetString("items",jsonItems);
            return View(cart);
        }
        public IActionResult Order()
        {
            string jsonEmail = HttpContext.Session.GetString("email");
            if (jsonEmail.IsNullOrEmpty())
                return RedirectToAction("Login"); // go to login controller if not login 
            string email = JsonSerializer.Deserialize<string>(jsonEmail);
            MyUser user = _context.Users.FirstOrDefault(u => u.Email == email);

            string jsonItems = HttpContext.Session.GetString("items");
            List<CartItem> cartItems = JsonSerializer.Deserialize<List<CartItem>>(jsonItems);
            Cart cart = new Cart { CartItems = cartItems };
            // Detach existing CartItems to avoid re-inserting them
            foreach (var item in cartItems)
            {
                _context.Entry(item).State = EntityState.Unchanged; // Ensure EF Core doesn't try to insert these
            }

            // Create and add Order
            Order order = new Order
            {
                User = user,
                Cart = cart
            };
            order.Cart.SetTotalPrice();
            _context.Carts.Add(order.Cart);
            _context.Orders.Add(order);
            _context.SaveChanges();

            Order order1 = _context.Orders.FirstOrDefault();
            HttpContext.Session.SetString("orderStatus", "placed");
            TempData["Status"] = "Order Placed";

            //TempData["Status"] = "Order Placed";
            // HttpContext.Session.Remove("items"); // Clear cart after placing the order
            return RedirectToAction("Cart", "MarketPlace");
        }

        public IActionResult OrdersList()
        {
            List<Order> orders = _context.Orders.ToList() ?? new List<Order> { };
            return View(orders);
        }
    }
}
