using Microsoft.AspNetCore.Mvc;

namespace HouseRepairApp.Controllers
{
	public class ContactUsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Booking()
		{
			return View();
		}
	}
}
