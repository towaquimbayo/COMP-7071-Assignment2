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
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Attendances.Include(a => a.Employee).Include(a => a.Shift);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .Include(a => a.Shift)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "Id", "Id");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,ShiftId,ApprovedByManager,Notes")] Attendance attendance)
        {
            // Remove validation for navigation properties
            ModelState.Remove("Employee");
            ModelState.Remove("Shift");

            if (ModelState.IsValid)
            {
                attendance.Id = Guid.NewGuid();
                _context.Add(attendance);
                await _context.SaveChangesAsync();

                try
                {
                    // Try to send email, but continue even if it fails
                    EmailService.NotifyAttendanceChange(attendance);
                }
                catch (Exception ex)
                {
                    // Log the error but don't stop the process
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    // Optionally add to ModelState or TempData if you want to show a message to the user
                    // TempData["EmailError"] = "Attendance was created but email notification failed.";
                }
                try
                {
                    // Enhanced logging for email service testing
                    Console.WriteLine("-----BEGIN EMAIL SERVICE TEST [Create]-----");
                    Console.WriteLine($"Timestamp: {DateTime.Now}");
                    Console.WriteLine($"Attempting to email attendance update for Employee {attendance.EmployeeId} on Shift {attendance.ShiftId}");

                    EmailService.NotifyAttendanceChange(attendance);

                    Console.WriteLine("Email service call completed successfully");
                    Console.WriteLine("-----END EMAIL SERVICE TEST [Create]-----");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"-----EMAIL SERVICE ERROR [Create]-----");
                    Console.WriteLine($"Timestamp: {DateTime.Now}");
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    Console.WriteLine($"-----END EMAIL SERVICE ERROR [Create]-----");
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", attendance.EmployeeId);
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "Id", "Id", attendance.ShiftId);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", attendance.EmployeeId);
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "Id", "Id", attendance.ShiftId);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,EmployeeId,ShiftId,ApprovedByManager,Notes")] Attendance attendance)
        {
            // Remove validation for navigation properties
            ModelState.Remove("Employee");
            ModelState.Remove("Shift");

            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();

                    try
                    {
                        // Try to send email, but continue even if it fails
                        EmailService.NotifyAttendanceChange(attendance);
                    }
                    catch (Exception ex)
                    {
                        // Log the error but don't stop the process
                        Console.WriteLine($"Error sending email: {ex.Message}");
                        // Optionally add to ModelState or TempData if you want to show a message to the user
                        // TempData["EmailError"] = "Attendance was created but email notification failed.";
                    }
                    try
                    {
                        // Enhanced logging for email service testing
                        Console.WriteLine("-----BEGIN EMAIL SERVICE TEST [Create]-----");
                        Console.WriteLine($"Timestamp: {DateTime.Now}");
                        Console.WriteLine($"Attempting to email attendance update for Employee {attendance.EmployeeId} on Shift {attendance.ShiftId}");

                        EmailService.NotifyAttendanceChange(attendance);

                        Console.WriteLine("Email service call completed successfully");
                        Console.WriteLine("-----END EMAIL SERVICE TEST [Create]-----");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"-----EMAIL SERVICE ERROR [Create]-----");
                        Console.WriteLine($"Timestamp: {DateTime.Now}");
                        Console.WriteLine($"Error sending email: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                        Console.WriteLine($"-----END EMAIL SERVICE ERROR [Create]-----");
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Name", attendance.EmployeeId);
            ViewData["ShiftId"] = new SelectList(_context.Shifts, "Id", "Id", attendance.ShiftId);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.Employee)
                .Include(a => a.Shift)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(Guid id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }
    }
}
