using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _7071Sprint1Demo.Data;
using _7071Sprint1Demo.Models;

namespace _7071Sprint1Demo.Controllers
{
    public class RentHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RentHistoryController> _logger;

        public RentHistoryController(ApplicationDbContext context, ILogger<RentHistoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var history = await _context.RentHistories
                                        .Include(o => o.Asset)
                                        .Include(o => o.Renter)
                                        .ToListAsync();
            return View(history);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var rentHistory = await _context.RentHistories
                                            .Include(o => o.Asset)
                                            .Include(o => o.Renter)
                                            .FirstOrDefaultAsync(o => o.Id == id);
            if (rentHistory == null)
            {
                return NotFound();
            }

            return View(rentHistory);
        }

        // GET: RentHistory/Create
        public IActionResult Create()
        {
            // Check if there are any assets or renters available
            if (!_context.Assets.Any() || !_context.Renters.Any())
            {
                ViewBag.Message = "No assets or renters available. Please add them before creating rent history.";
            }

            ViewBag.AssetList = new SelectList(_context.Assets
                                           .GroupBy(a => a.Id)
                                           .Select(g => g.First()), "Id", "Type");

            ViewBag.RenterList = new SelectList(_context.Renters
                                                        .GroupBy(r => r.Id)
                                                        .Select(g => g.First()), "Id", "Name");
            return View();
        }

        // POST: RentHistory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssetId,RenterId,OldRentAmount,NewRentAmount,EffectiveDate")] RentHistory rentHistory)
        {
            // Remove validation for navigation properties
            ModelState.Remove("Asset");
            ModelState.Remove("Renter");

            if (ModelState.IsValid)
            {
                rentHistory.Id = Guid.NewGuid(); // Ensure the Id is created for the new record
                _context.Add(rentHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Repopulate dropdown lists in case of validation failure
            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Name", rentHistory.AssetId);
            ViewData["RenterId"] = new SelectList(_context.Renters, "Id", "Name", rentHistory.RenterId);
            return View(rentHistory);
        }

        // GET: RentHistory/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var rentHistory = await _context.RentHistories
                                            .Include(o => o.Asset)
                                            .Include(o => o.Renter)
                                            .FirstOrDefaultAsync(o => o.Id == id);
            if (rentHistory == null)
            {
                return NotFound();
            }
            ViewBag.AssetList = new SelectList(_context.Assets
                                                      .GroupBy(a => a.Id)
                                                      .Select(g => g.First()), "Id", "Type");

            ViewBag.RenterList = new SelectList(_context.Renters
                                                        .GroupBy(r => r.Id)
                                                        .Select(g => g.First()), "Id", "Name");
            return View(rentHistory);
        }

        // POST: RentHistory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AssetId,RenterId,OldRentAmount,NewRentAmount,EffectiveDate")] RentHistory rentHistory)
        {
            if (id != rentHistory.Id)
            {
                return NotFound();
            }

            // Remove validation for navigation properties
            ModelState.Remove("Asset");
            ModelState.Remove("Renter");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError("Concurrency error occurred while updating rent history: {Message}", ex.Message);
                    if (!RentHistoryExists(rentHistory.Id))
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

            ViewData["AssetId"] = new SelectList(_context.Assets, "Id", "Name", rentHistory.AssetId);
            ViewData["RenterId"] = new SelectList(_context.Renters, "Id", "Name", rentHistory.RenterId);
            return View(rentHistory);
        }

        // GET: RentHistory/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var rentHistory = await _context.RentHistories
                                            .Include(o => o.Asset)
                                            .Include(o => o.Renter)
                                            .FirstOrDefaultAsync(o => o.Id == id);
            if (rentHistory == null)
            {
                return NotFound();
            }

            return View(rentHistory);
        }

        // POST: RentHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rentHistory = await _context.RentHistories.FindAsync(id);
            if (rentHistory == null)
            {
                return NotFound();
            }

            _context.RentHistories.Remove(rentHistory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentHistoryExists(Guid id)
        {
            return _context.RentHistories.Any(e => e.Id == id);
        }
    }
}
