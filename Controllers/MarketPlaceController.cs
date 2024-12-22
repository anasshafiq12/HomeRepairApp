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
                ids.Add(id);
                string jsonIds = JsonSerializer.Serialize(ids);
                HttpContext.Session.SetString("ids", jsonIds);
            }

            string jsonItems = HttpContext.Session.GetString("cartItems");
            List<CartItem> items = JsonSerializer.Deserialize<List<CartItem>>(jsonItems) ?? new List<CartItem>();

            return View(items);
        }
        public IActionResult Cart()
        {
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
            return View(cart);
        }
        [HttpPost]
        public IActionResult Cart(int id, int quantity)
        {
            string json = HttpContext.Session.GetString("items");
            List<CartItem> items = JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
            Cart cart = new Cart { CartItems = items };
            cart.SetItemPrice(id, quantity);
            cart.SetTotalPrice();
            return View(cart);
        }

    }
}
