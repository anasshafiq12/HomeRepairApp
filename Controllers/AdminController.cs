﻿using HouseRepairApp.Data;
using HouseRepairApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseRepairApp.Controllers
{
	public class AdminController : Controller
	{
        private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _context;
        public AdminController(IWebHostEnvironment env, ApplicationDbContext context)
        {
            _context = context;
			_environment = env;
        }
        public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Index(CartItem item,IFormFile image)
		{
			if (item == null || image == null)
			{
				ModelState.AddModelError(string.Empty, "Invalid input data. Please ensure all fields are filled.");
				return View(item);
			}
			try
			{
                string wwwPath = _environment.WebRootPath;
                string uploadsPath = Path.Combine(wwwPath, "uploads");
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // Generate unique file name to avoid overwrites
                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(uploadsPath, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                // Save image URL or relative path to the item
                item.ImageUrl = Path.Combine("uploads", fileName);
                _context.CartItems.Add(item);
				_context.SaveChanges();
				return View();
			}
			catch (Exception ex)
			{
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request: " + ex.Message);
                return View(item);
            }
		}
		public IActionResult Bookings()
		{
			List<Booking> bookings = _context.Bookings.ToList();
			return View(bookings);
		}
		[HttpPost]
		public IActionResult Delete(int id)
		{
            Booking booking = _context.Bookings.FirstOrDefault(z => z.Id == id); 
			_context.Bookings.Remove(booking);
            _context.SaveChanges();
            return RedirectToAction("Bookings", "Admin");
		}
	}
}
