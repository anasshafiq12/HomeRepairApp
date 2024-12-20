using Microsoft.AspNetCore.Mvc;

namespace HouseRepairApp.Controllers
{
	public class AboutUsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
