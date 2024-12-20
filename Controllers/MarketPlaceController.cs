using Microsoft.AspNetCore.Mvc;

namespace HouseRepairApp.Controllers
{
	public class MarketPlaceController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Cart()
		{
			return View();
		}
	}
}
