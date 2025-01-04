using HouseRepairApp.Data;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.Authorization;
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
           // Console.WriteLine(id);
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
                    string jsonids = JsonSerializer.Serialize(ids);
                    HttpContext.Session.SetString("ids", jsonids);
                }
            }

            string jsonItems = HttpContext.Session.GetString("cartItems");
            List<CartItem> items = JsonSerializer.Deserialize<List<CartItem>>(jsonItems) ?? new List<CartItem>();
            return View(items);
        }
        public IActionResult Cart()
        {
            // If items already selected
            if (User.Identity.IsAuthenticated)
            {
                if (HttpContext.Session.Keys.Contains("items"))
                {
                    string jsonitems = HttpContext.Session.GetString("items");
                    List<CartItem> cartItems = JsonSerializer.Deserialize<List<CartItem>>(jsonitems) ?? new List<CartItem>();
                    List<string> CheckIds = new List<string>();
                    foreach (var item in cartItems)
                    {
                        CheckIds.Add(item.ItemId.ToString());
                    }
                    string jsonIds = HttpContext.Session.GetString("ids");
                    List<string> idsList = JsonSerializer.Deserialize<List<string>>(jsonIds);
                    //   List<CartItem> cartItems = new List<CartItem>();
                    foreach (string id in idsList)
                    {
                        if (!CheckIds.Contains(id))
                        {
                            CartItem item = _context.CartItems.FirstOrDefault(z => z.ItemId == int.Parse(id));
                            cartItems.Add(item);
                        }
                    }
                    string jsonItemsP = JsonSerializer.Serialize(cartItems);
                    HttpContext.Session.SetString("items", jsonItemsP);
                    List<SelectedCartItem> selectedCartItems1 = new List<SelectedCartItem>();
                    foreach (var cartItem in cartItems)
                    {
                        SelectedCartItem item = new SelectedCartItem();
                        item.ItemId = cartItem.ItemId;
                        item.Name = cartItem.Name;
                        item.Price = cartItem.Price;
                        item.Description = cartItem.Description;
                        item.Category = cartItem.Category;
                        item.SelectedQuantityByUser = cartItem.SelectedQuantityByUser;
                        item.ImageUrl = cartItem.ImageUrl;
                        item.AvailableQuantity = cartItem.AvailableQuantity;
                        item.PriceWrtQuanity = cartItem.PriceWrtQuanity;

                        selectedCartItems1.Add(item);
                    }
                    Cart cartP = new Cart { SelectedCartItems = selectedCartItems1 };
                    cartP.SetTotalPrice();
                    if (HttpContext.Session.Keys.Contains("orderStatus"))
                    {

                        ViewBag.Status = "Order Placed"; // Pass TempData to the view via ViewBag

                    }
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
                List<SelectedCartItem> selectedCartItems = new List<SelectedCartItem>();
                foreach (var cartItem in items)
                {
                    SelectedCartItem item = new SelectedCartItem();
                    item.ItemId = cartItem.ItemId;
                    item.Name = cartItem.Name;
                    item.Price = cartItem.Price;
                    item.Description = cartItem.Description;
                    item.Category = cartItem.Category;
                    item.SelectedQuantityByUser = cartItem.SelectedQuantityByUser;
                    item.ImageUrl = cartItem.ImageUrl;
                    item.PriceWrtQuanity = cartItem.PriceWrtQuanity;
                    selectedCartItems.Add(item);
                }
                Cart cart = new Cart { SelectedCartItems = selectedCartItems };
                cart.SetTotalPrice();
                ViewBag.Status = TempData["Status"]; // Pass TempData to the view via ViewBag
                return View(cart);
            }
            return Redirect("/Identity/Account/Login");
        }
        [HttpPost]
        //public IActionResult Cart(int id, int quantity)
        public IActionResult Cart([FromBody] UpdateQuantityRequest request)
        {
            if (User.Identity.IsAuthenticated)
            {
                string json = HttpContext.Session.GetString("items");
                List<CartItem> items = JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
                List<SelectedCartItem> selectedCartItems = new List<SelectedCartItem>();
                foreach (var cartItem in items)
                {
                    SelectedCartItem item = new SelectedCartItem();
                    item.ItemId = cartItem.ItemId;
                    item.Name = cartItem.Name;
                    item.Price = cartItem.Price;
                    item.Description = cartItem.Description;
                    item.Category = cartItem.Category;
                    item.SelectedQuantityByUser = cartItem.SelectedQuantityByUser;
                    item.ImageUrl = cartItem.ImageUrl;
                    item.PriceWrtQuanity = cartItem.PriceWrtQuanity;
                    selectedCartItems.Add(item);
                }
                Cart cart = new Cart { SelectedCartItems = selectedCartItems };
                if (request.Quantity == 0)
                {

                    selectedCartItems.RemoveAll(z => z.ItemId == request.Id);
                    string jsonIds = HttpContext.Session.GetString("ids");
                    List<string> ids = JsonSerializer.Deserialize<List<string>>(jsonIds);
                    string productId = request.Id.ToString();
                    if (ids.Remove(productId))
                    {
                        jsonIds = JsonSerializer.Serialize(ids);
                        HttpContext.Session.SetString("ids", jsonIds);
                    }
                }
                cart.SetItemPriceAndQuantity(request.Id, request.Quantity);
                cart.SetTotalPrice();
                List<CartItem> cartItems = new List<CartItem>();
                foreach (var cartItem in selectedCartItems)
                {
                    CartItem item = new CartItem();
                    item.ItemId = cartItem.ItemId;
                    item.Name = cartItem.Name;
                    item.Price = cartItem.Price;
                    item.Description = cartItem.Description;
                    item.Category = cartItem.Category;
                    item.SelectedQuantityByUser = cartItem.SelectedQuantityByUser;
                    item.ImageUrl = cartItem.ImageUrl;
                    item.PriceWrtQuanity = cartItem.PriceWrtQuanity;
                    cartItems.Add(item);
                }
                string jsonItems = JsonSerializer.Serialize(cartItems);
                HttpContext.Session.SetString("items", jsonItems);
                //return View(cart);
                return PartialView("_CartItemPartial", cart);
            }
            return Redirect("/Identity/Account/Login");
        }

        public IActionResult Order()
        {
            string jsonEmail = HttpContext.Session.GetString("email");
            if (jsonEmail.IsNullOrEmpty())
                return Redirect("/Identity/Account/Login");
            string email = JsonSerializer.Deserialize<string>(jsonEmail);
            MyUser user = _context.Users.FirstOrDefault(u => u.Email == email);

            string jsonItems = HttpContext.Session.GetString("items");
            List<CartItem> cartItems = JsonSerializer.Deserialize<List<CartItem>>(jsonItems);

            List<SelectedCartItem> selectedCartItems = new List<SelectedCartItem>();
            foreach (var cartItem in cartItems)
            {
                SelectedCartItem item = new SelectedCartItem();
                item.Name = cartItem.Name;
                item.Price = cartItem.Price;
                item.Description = cartItem.Description;
                item.Category = cartItem.Category;
                item.SelectedQuantityByUser = cartItem.SelectedQuantityByUser;
                item.ImageUrl = cartItem.ImageUrl;
                item.PriceWrtQuanity = cartItem.PriceWrtQuanity;
                selectedCartItems.Add(item);
            }
            Cart cart = new Cart { SelectedCartItems = selectedCartItems };      
            
            // Create and add Order
            cart.SetTotalPrice();
            cart.UserId = user.Id;
            _context.Carts.Add(cart);
            _context.SaveChanges();
            Cart cartId = _context.Carts.FirstOrDefault(u => u.UserId == cart.UserId);
            HttpContext.Session.SetString("orderStatus", "placed");
            List<SelectedCartItem> selectedCartItems2 = new List<SelectedCartItem>();
            foreach(var  cartItem in cartItems)
            {
                SelectedCartItem item = new SelectedCartItem();
                item.Name = cartItem.Name;
                item.Price = cartItem.Price;
                item.Description = cartItem.Description;
                item.Category = cartItem.Category;
                item.SelectedQuantityByUser = cartItem.SelectedQuantityByUser; 
                item.ImageUrl = cartItem.ImageUrl;
                item.CartId = cartId.CartId;
                item.PriceWrtQuanity = cartItem.PriceWrtQuanity;
                selectedCartItems2.Add(item);
            }

            _context.SelectedCartItems.AddRange(selectedCartItems2);
            _context.SaveChanges();
            //TempData["Status"] = "Order Placed";
            // HttpContext.Session.Remove("items"); // Clear cart after placing the order
            return RedirectToAction("Cart", "MarketPlace");
        }

        public IActionResult OrdersList()
        {
            //List<Order> orders = _context.Orders.ToList() ?? new List<Order> { };
            List<Cart> carts = _context.Carts.ToList();
            return View(carts);
        }
    }
}
