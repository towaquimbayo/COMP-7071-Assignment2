using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment2.Server.Data;
using Assignment2.Server.Models;

namespace Assignment2.Server.Controllers
{
    public class ServiceRegistrationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceRegistrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceRegistrations/Create
        public IActionResult Create()
        {
            // Create SelectLists for the dropdowns
            ViewBag.Services = new SelectList(_context.Services.ToList(), "Id", "Name");
            ViewBag.Clients = new SelectList(_context.Clients.ToList(), "Id", "Name"); // Assumes Client has a "Name" property
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceBooking booking)
        {
            if (ModelState.IsValid)
            {
                booking.Id = Guid.NewGuid();
                _context.ServiceBookings.Add(booking);
                _context.SaveChanges();
                return RedirectToAction("Confirmation");
            }
            ViewBag.Services = new SelectList(_context.Services.ToList(), "Id", "Name");
            ViewBag.Clients = new SelectList(_context.Clients.ToList(), "Id", "Name");
            return View(booking);
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
