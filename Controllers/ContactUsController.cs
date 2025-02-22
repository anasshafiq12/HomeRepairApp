using HouseRepairApp.Data;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Security.Claims;

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
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
			ViewBag.Name = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            if (!MessageDictionary.Messagedictionary.TryGetValue(email, out List<Message> messagesList))
            {
                messagesList = new List<Message>();
            }

            //  IEnumerable<Message> messages = messagesList.Value ?? new IEnumerable<Message>();

            return View(messagesList);
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
