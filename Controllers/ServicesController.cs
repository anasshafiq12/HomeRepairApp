using Microsoft.AspNetCore.Mvc;

namespace HouseRepairApp.Controllers
{
	public class ServicesController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
