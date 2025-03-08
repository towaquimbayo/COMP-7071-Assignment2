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
    public class OccupancyHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OccupancyHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all occupancy histories
        public async Task<IActionResult> Index()
        {
            var history = await _context.OccupancyHistories
                                        .Include(o => o.Asset)
                                        .Include(o => o.Renter)
                                        .ToListAsync();
            return View(history);
        }

        // View a specific occupancy history
        public async Task<IActionResult> Details(Guid id)
        {
            var occupancyHistory = await _context.OccupancyHistories
                .Include(o => o.Asset)
                .Include(o => o.Renter)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (occupancyHistory == null)
            {
                return NotFound();
            }

            return View(occupancyHistory);
        }

        // GET: Create new occupancy history
        public IActionResult Create()
        {
            ViewBag.AssetList = new SelectList(_context.Assets
                                           .GroupBy(a => a.Id)
                                           .Select(g => g.First()), "Id", "Type");

            ViewBag.RenterList = new SelectList(_context.Renters
                                                        .GroupBy(r => r.Id)
                                                        .Select(g => g.First()), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssetId,RenterId,StartDate,EndDate")] OccupancyHistory occupancyHistory)
        {   
            // Remove validation for navigation properties
            ModelState.Remove("Asset");
            ModelState.Remove("Renter");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");

                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }

                // Repopulate dropdowns
                ViewBag.AssetList = new SelectList(_context.Assets, "Id", "Type", occupancyHistory.AssetId);
                ViewBag.RenterList = new SelectList(_context.Renters, "Id", "Name", occupancyHistory.RenterId);

                return View(occupancyHistory);
            }

            occupancyHistory.Id = Guid.NewGuid();
            _context.Add(occupancyHistory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        /// GET: Edit an occupancy history record
        public async Task<IActionResult> Edit(Guid id)
        {
            var occupancyHistory = await _context.OccupancyHistories
                                                 .Include(o => o.Asset)
                                                 .Include(o => o.Renter)
                                                 .FirstOrDefaultAsync(o => o.Id == id);
            if (occupancyHistory == null)
            {
                return NotFound();
            }

            ViewBag.AssetList = new SelectList(_context.Assets
                                           .GroupBy(a => a.Id)
                                           .Select(g => g.First()), "Id", "Type");

            ViewBag.RenterList = new SelectList(_context.Renters
                                                        .GroupBy(r => r.Id)
                                                        .Select(g => g.First()), "Id", "Name");

            return View(occupancyHistory);
        }

        //Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AssetId,RenterId,StartDate,EndDate")] OccupancyHistory occupancyHistory)
        {
            if (id != occupancyHistory.Id)
            {
                return NotFound();
            }
            // Remove validation for navigation properties
            ModelState.Remove("Asset");
            ModelState.Remove("Renter");

            if (!ModelState.IsValid)
            {
                // Log validation errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                // Repopulate dropdowns
                ViewBag.AssetList = new SelectList(_context.Assets, "Id", "Type", occupancyHistory.AssetId);
                ViewBag.RenterList = new SelectList(_context.Renters, "Id", "Name", occupancyHistory.RenterId);

                return View(occupancyHistory);
            }

            try
            {
                _context.Update(occupancyHistory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.OccupancyHistories.Any(e => e.Id == id))
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


        // GET: Delete occupancy history confirmation
        public async Task<IActionResult> Delete(Guid id)
        {
            var occupancyHistory = await _context.OccupancyHistories
                .Include(o => o.Asset)
                .Include(o => o.Renter)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (occupancyHistory == null)
            {
                return NotFound();
            }

            return View(occupancyHistory);
        }

        // POST: Delete occupancy history
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var occupancyHistory = await _context.OccupancyHistories.FindAsync(id);
            if (occupancyHistory != null)
            {
                _context.OccupancyHistories.Remove(occupancyHistory);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

