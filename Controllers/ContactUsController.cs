using HouseRepairApp.Data;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace HouseRepairApp.Controllers
{
	public class ContactUsController : Controller
	{
        private readonly ApplicationDbContext _context;
        public ContactUsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
		{
			return View();
		}
		public IActionResult Booking()
		{
			if(!User.Identity.IsAuthenticated)
                return Redirect("/Identity/Account/Login");
            return View();
		}
		public IActionResult Chat()
		{

			return View();
		}
		[HttpPost]
		public IActionResult Booking(Booking booking)
		{
			_context.Bookings.Add(booking);
			_context.SaveChanges();
			return View();
		}
	}
}
