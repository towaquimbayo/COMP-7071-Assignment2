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
    public class HiringController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HiringController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hiring
        public async Task<IActionResult> Index()
        {
            return View(await _context.Applicants.ToListAsync());
        }

        // GET: Hiring/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        // GET: Hiring/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hiring/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Address,Email,Phone,EmergencyContact,ApplicationStatus,DateApplied,ApplicantType,PositionAppliedFor,Resume,PreferredUnitType,IncomeVerification")] Applicant applicant)
        {
            if (ModelState.IsValid)
            {
                applicant.Id = Guid.NewGuid();
                _context.Add(applicant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(applicant);
        }

        // GET: Hiring/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null)
            {
                return NotFound();
            }
            return View(applicant);
        }

        // POST: Hiring/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Address,Email,Phone,EmergencyContact,ApplicationStatus,DateApplied,ApplicantType,PositionAppliedFor,Resume,PreferredUnitType,IncomeVerification")] Applicant applicant)
        {
            if (id != applicant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantExists(applicant.Id))
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
            return View(applicant);
        }

        // GET: Hiring/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        // POST: Hiring/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant != null)
            {
                _context.Applicants.Remove(applicant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicantExists(Guid id)
        {
            return _context.Applicants.Any(e => e.Id == id);
        }


        // GET: Hiring/Application
        // Show a form for the applicant to fill out their info.
        public IActionResult Application()
        {
            return View();
        }

        // POST: Hiring/Application
        // Submit applicant info. Creates a new Applicant in the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Application([Bind("Name,Email,ResumePath,PositionApplied")] Applicant applicant)
        {
            if (ModelState.IsValid)
            {
                applicant.Id = Guid.NewGuid();
                if (applicant.DateApplied == default)
                {
                    applicant.DateApplied = DateTime.Now;
                }
                _context.Applicants.Add(applicant);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Interview), new { id = applicant.Id });
            }
            return View(applicant);
        }

        // GET: Hiring/Interview/{id}
        // Display an interview scheduling page for the given applicant.
        public async Task<IActionResult> Interview(Guid id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null)
            {
                return NotFound();
            }
            return View(applicant);
        }

        // POST: Hiring/ScheduleInterview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ScheduleInterview(Guid applicantId, DateTime scheduledDate)
        {
            var applicant = await _context.Applicants.FindAsync(applicantId);
            if (applicant == null)
            {
                return NotFound();
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = applicant.Id });
        }

        // GET: Hiring/Onboarding/{id}
        // Once the applicant is accepted, convert them to an Employee and display a success page.
        public async Task<IActionResult> Onboarding(Guid id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            if (applicant == null)
            {
                return NotFound();
            }

            var newEmployee = new Employee
            {
                Id = Guid.NewGuid(),
                Name = applicant.Name,

                Address = applicant.Address,
                EmergencyContact = applicant.EmergencyContact,

                JobTitle = applicant.PositionAppliedFor ?? "New Hire",

                PayRate = 0m,
            };

            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();

            return View(newEmployee);
        }

    }
}
