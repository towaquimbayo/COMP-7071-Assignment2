using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Assignment2.Server.Data;
using Assignment2.Server.Models;

namespace Assignment2.Server.Controllers
{
    public class ServiceScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceSchedule/Create
        public IActionResult Create()
        {
            // Create a SelectList for services and a MultiSelectList for employees.
            ViewBag.Services = new SelectList(_context.Services.ToList(), "Id", "Name");
            ViewBag.Employees = new MultiSelectList(_context.Employees.ToList(), "Id", "Name"); // Assumes Employee has a "Name" property
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceBooking booking, string[] AssignedEmployees)
        {
            if (ModelState.IsValid)
            {
                booking.Id = Guid.NewGuid();

                // If assigned employees are selected, attach them to the booking.
                if (AssignedEmployees != null && AssignedEmployees.Length > 0)
                {
                    booking.AssignedEmployees = _context.Employees
                        .Where(e => AssignedEmployees.Contains(e.Id.ToString()))
                        .ToList();
                }

                _context.ServiceBookings.Add(booking);
                _context.SaveChanges();
                return RedirectToAction("Index", "Services");
            }
            ViewBag.Services = new SelectList(_context.Services.ToList(), "Id", "Name");
            ViewBag.Employees = new MultiSelectList(_context.Employees.ToList(), "Id", "Name");
            return View(booking);
        }
    }
}
