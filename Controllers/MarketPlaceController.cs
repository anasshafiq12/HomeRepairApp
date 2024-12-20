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
			_context.CartItems.Add(item);
			return View();
		}
		public IActionResult Cart()
		{
			var items = _context.CartItems;
			return View(items);
		}
	}
}
