using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _7071Sprint1Demo.Data;
using _7071Sprint1Demo.Models;

namespace _7071Sprint1Demo.Controllers
{
    public class ServiceScheduleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceScheduleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceSchedule
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.ServiceBookings
                .Include(s => s.Client)
                .Include(s => s.Service)
                .Include(s => s.AssignedEmployees)
                .ToListAsync();

            return View(bookings);
        }

        // GET: ServiceSchedule/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceBooking = await _context.ServiceBookings
                .Include(s => s.Client)
                .Include(s => s.Service)
                .Include(s => s.AssignedEmployees)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (serviceBooking == null)
            {
                return NotFound();
            }

            return View(serviceBooking);
        }

        // GET: ServiceSchedule/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name");

            return View();
        }

        // POST: ServiceSchedule/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Guid ServiceId, DateTime? ScheduledDate, Guid EmployeeId)
        {
            // Get the first client from the database
            var client = await _context.Clients.FirstOrDefaultAsync();

            // Create the service booking
            var serviceBooking = new ServiceBooking
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,  // Use the first client's ID
                ServiceId = ServiceId,
                ScheduledDate = ScheduledDate
            };

            // Add to database and save
            _context.ServiceBookings.Add(serviceBooking);
            await _context.SaveChangesAsync();

            // Handle employee assignment
            var employee = await _context.Employees.FindAsync(EmployeeId);
            if (employee != null)
            {
                // Ensure the navigation property is loaded
                await _context.Entry(serviceBooking)
                    .Collection(sb => sb.AssignedEmployees)
                    .LoadAsync();

                // Add employee to the booking
                serviceBooking.AssignedEmployees.Add(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceSchedule/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceBooking = await _context.ServiceBookings
                .Include(sb => sb.AssignedEmployees)
                .FirstOrDefaultAsync(sb => sb.Id == id);

            if (serviceBooking == null)
            {
                return NotFound();
            }

            // Get assigned employee IDs
            var assignedEmployeeIds = serviceBooking.AssignedEmployees
                .Select(e => e.Id)
                .ToList();

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", serviceBooking.ClientId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", serviceBooking.ServiceId);
            ViewData["EmployeeIds"] = new MultiSelectList(_context.Employees, "Id", "Name", assignedEmployeeIds);

            return View(serviceBooking);
        }

        // POST: ServiceSchedule/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ServiceId,ScheduledDate")] ServiceBooking serviceBooking, List<Guid> EmployeeIds)
        {
            // Get existing booking
            var existingBooking = await _context.ServiceBookings
                .Include(sb => sb.AssignedEmployees)
                .FirstOrDefaultAsync(sb => sb.Id == id);

            if (existingBooking == null)
            {
                return NotFound();
            }

            // Update fields
            existingBooking.ServiceId = serviceBooking.ServiceId;
            existingBooking.ScheduledDate = serviceBooking.ScheduledDate;

            // Update employee assignments
            existingBooking.AssignedEmployees.Clear();

            if (EmployeeIds != null && EmployeeIds.Any())
            {
                foreach (var employeeId in EmployeeIds)
                {
                    var employee = await _context.Employees.FindAsync(employeeId);
                    if (employee != null)
                    {
                        existingBooking.AssignedEmployees.Add(employee);
                    }
                }
            }

            // Save changes
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: ServiceSchedule/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceBooking = await _context.ServiceBookings
                .Include(s => s.Client)
                .Include(s => s.Service)
                .Include(s => s.AssignedEmployees)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (serviceBooking == null)
            {
                return NotFound();
            }

            return View(serviceBooking);
        }

        // POST: ServiceSchedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var serviceBooking = await _context.ServiceBookings
                .Include(sb => sb.AssignedEmployees)
                .FirstOrDefaultAsync(sb => sb.Id == id);

            if (serviceBooking != null)
            {
                // Clear employee assignments
                serviceBooking.AssignedEmployees.Clear();

                // Remove booking
                _context.ServiceBookings.Remove(serviceBooking);

                // Save changes
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}