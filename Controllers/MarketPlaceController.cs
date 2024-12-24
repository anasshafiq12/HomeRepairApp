using HouseRepairApp.Data;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                ViewBag.Status = TempData["Status"]; // Pass TempData to the view via ViewBag
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
            //string jsonEmail = HttpContext.Session.GetString("email");
            //string email = JsonSerializer.Deserialize<string>(jsonEmail).ToString();
            //MyUser user = _context.Users.FirstOrDefault( z => z.Email == email);
            //string jsonItems = HttpContext.Session.GetString("items");
            //List<CartItem> cartItems = JsonSerializer.Deserialize<List<CartItem>>(jsonItems);
            //Order order = new Order { User = user, CartItems = cartItems };
            //_context.Orders.Add(order);
            TempData["Status"] = "Order Placed";
            return RedirectToAction("Cart","MarketPlace");
        }
    }
}
