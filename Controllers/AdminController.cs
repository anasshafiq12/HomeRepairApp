using Microsoft.AspNetCore.Mvc;

namespace HouseRepairApp.Controllers
{
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Bookings()
		{
			return View();
		}
	}
}
