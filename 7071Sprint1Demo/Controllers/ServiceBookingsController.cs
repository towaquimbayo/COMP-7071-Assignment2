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
    public class ServiceBookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceBookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ServiceBookings
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ServiceBookings.Include(s => s.Client).Include(s => s.Service);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ServiceBookings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceBooking = await _context.ServiceBookings
                .Include(s => s.Client)
                .Include(s => s.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceBooking == null)
            {
                return NotFound();
            }

            return View(serviceBooking);
        }

        // GET: ServiceBookings/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // POST: ServiceBookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,ServiceId,ScheduledDate")] ServiceBooking serviceBooking)
        {
            // Remove validation for navigation properties
            ModelState.Remove("Client");
            ModelState.Remove("Service");

            Console.WriteLine($"Form submitted - ClientId: {serviceBooking.ClientId}, ServiceId: {serviceBooking.ServiceId}, Date: {serviceBooking.ScheduledDate}");

            if (ModelState.IsValid)
            {
                try
                {
                    // Generate new GUID
                    serviceBooking.Id = Guid.NewGuid();

                    // Add to context and save
                    _context.ServiceBookings.Add(serviceBooking);
                    var result = await _context.SaveChangesAsync();
                    Console.WriteLine($"Rows affected: {result}");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    ModelState.AddModelError("", $"Unable to save changes: {ex.Message}");
                }
            }
            else
            {
                // Log validation errors
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        Console.WriteLine($"Error in {state.Key}: {string.Join(", ", state.Value.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", serviceBooking.ClientId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", serviceBooking.ServiceId);
            return View(serviceBooking);
        }

        // GET: ServiceBookings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceBooking = await _context.ServiceBookings.FindAsync(id);
            if (serviceBooking == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", serviceBooking.ClientId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", serviceBooking.ServiceId);
            return View(serviceBooking);
        }

        // POST: ServiceBookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ClientId,ServiceId,ScheduledDate")] ServiceBooking serviceBooking)
        {
            if (id != serviceBooking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceBooking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceBookingExists(serviceBooking.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", serviceBooking.ClientId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", serviceBooking.ServiceId);
            return View(serviceBooking);
        }

        // GET: ServiceBookings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceBooking = await _context.ServiceBookings
                .Include(s => s.Client)
                .Include(s => s.Service)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (serviceBooking == null)
            {
                return NotFound();
            }

            return View(serviceBooking);
        }

        // POST: ServiceBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var serviceBooking = await _context.ServiceBookings.FindAsync(id);
            if (serviceBooking != null)
            {
                _context.ServiceBookings.Remove(serviceBooking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceBookingExists(Guid id)
        {
            return _context.ServiceBookings.Any(e => e.Id == id);
        }
    }
}
