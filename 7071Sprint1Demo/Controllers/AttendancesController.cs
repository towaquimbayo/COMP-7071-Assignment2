﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _7071Sprint1Demo.Data;
using _7071Sprint1Demo.Models;
using EmailNotifier; 
using System.Collections.Generic;

namespace _7071Sprint1Demo.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailNotifier _emailNotifier;

        public AttendancesController(ApplicationDbContext context, IEmailNotifier emailNotifier)
        {
            _context = context;
            _emailNotifier = emailNotifier;
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
                    // Assume Employee has an Email property; otherwise use a placeholder or look it up appropriately.
                    var employee = await _context.Employees.FindAsync(attendance.EmployeeId);
                    await _emailNotifier.SendAttendanceNotificationAsync(attendance.Id, employee.Email, $"Attendance updated for shift {attendance.ShiftId}");
                    string details = $"Attendance updated for shift {attendance.ShiftId}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending attendance email: {ex.Message}");
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
                        var employee = await _context.Employees.FindAsync(attendance.EmployeeId);
                        string recipientEmail = employee != null ? employee.Email : "employee@example.com";
                        string details = $"Attendance updated for shift {attendance.ShiftId}";
                        await _emailNotifier.SendAttendanceNotificationAsync(attendance.Id, recipientEmail, details);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending attendance update email: {ex.Message}");
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

