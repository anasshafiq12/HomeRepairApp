using HouseRepairApp.Data;
using HouseRepairApp.Models;
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
		[HttpPost]
		public IActionResult Booking(Booking booking)
		{
			using (var context = new ApplicationDbContext())
			{

			}
			return View();
		}
	}
}
