using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _7071Sprint1Demo.Data;
using _7071Sprint1Demo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace _7071Sprint1Demo.Controllers
{
    public class RentInvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RentInvoiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all rent invoices.
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.RentInvoices
                                          .Include(i => i.Asset)
                                          .Include(i => i.Renter)
                                          .ToListAsync(); // Use ToListAsync() for async
            return View(invoices);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var invoice = await _context.RentInvoices
                                        .Include(i => i.Asset)   // Include Asset
                                        .Include(i => i.Renter)  // Include Renter
                                        .FirstOrDefaultAsync(i => i.Id == id); // Use FirstOrDefaultAsync to retrieve the invoice

            if (invoice == null)
                return NotFound();

            return View(invoice);
        }


        // GET: RentInvoice/Create
        public IActionResult Create()
        {
            // Fetch assets and renters for dropdown lists
            ViewBag.AssetList = new SelectList(_context.Assets
                                          .GroupBy(a => a.Id)
                                          .Select(g => g.First()), "Id", "Type");

            ViewBag.RenterList = new SelectList(_context.Renters
                                                        .GroupBy(r => r.Id)
                                                        .Select(g => g.First()), "Id", "Name");
            return View();
        }

        // POST: RentInvoice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssetId,RenterId,IssueDate,DueDate,AmountDue,IsPaid")] RentInvoice rentInvoice)
        {
            // Remove validation for navigation properties
            ModelState.Remove("Asset");
            ModelState.Remove("Renter");

            if (ModelState.IsValid)
            {
                rentInvoice.Id = Guid.NewGuid();
                _context.Add(rentInvoice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If validation fails, repopulate dropdown lists
            ViewBag.AssetList = new SelectList(_context.Assets, "Id", "Type");
            ViewBag.RenterList = new SelectList(_context.Renters, "Id", "Name");
            return View(rentInvoice);
        }



        // GET: RentInvoice/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var rentInvoice = await _context.RentInvoices
                                             .Include(i => i.Asset)
                                             .Include(i => i.Renter)
                                             .FirstOrDefaultAsync(i => i.Id == id);
            if (rentInvoice == null)
            {
                return NotFound();
            }

            // Fetch assets and renters for dropdown lists
            ViewBag.AssetList = new SelectList(_context.Assets
                                          .GroupBy(a => a.Id)
                                          .Select(g => g.First()), "Id", "Type");

            ViewBag.RenterList = new SelectList(_context.Renters
                                                        .GroupBy(r => r.Id)
                                                        .Select(g => g.First()), "Id", "Name");
            return View(rentInvoice);
        }

    // POST: RentInvoice/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id,AssetId,RenterId,IssueDate,DueDate,AmountDue,IsPaid")] RentInvoice rentInvoice)
    {
    if (id != rentInvoice.Id)
    {
        return NotFound();
    }
    // Remove validation for navigation properties
    ModelState.Remove("Asset");
    ModelState.Remove("Renter");

    if (ModelState.IsValid)
    {
            // Attach the entity to the context and set its state as Modified to handle updates.
            _context.Entry(rentInvoice).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
     }

 
        // Repopulate dropdown lists if the model state is invalid
        ViewBag.AssetList = new SelectList(_context.Assets, "Id", "Type", rentInvoice.AssetId);
        ViewBag.RenterList = new SelectList(_context.Renters, "Id", "Name", rentInvoice.RenterId);
        return View(rentInvoice);
    }

        // GET: RentInvoice/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return NotFound();
            }

            var rentInvoice = await _context.RentInvoices
                                             .Include(i => i.Asset)
                                             .Include(i => i.Renter)
                                             .FirstOrDefaultAsync(i => i.Id == id);
            if (rentInvoice == null)
            {
                return NotFound();
            }

            return View(rentInvoice);
        }

        // POST: RentInvoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var rentInvoice = await _context.RentInvoices.FindAsync(id);
            if (rentInvoice == null)
            {
                return NotFound();
            }

            _context.RentInvoices.Remove(rentInvoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // Email a rent invoice (if applicable, needs implementation).
        public ActionResult Email(Guid id)
        {
            var invoice = _context.RentInvoices.Find(id);
            if (invoice == null)
                return NotFound();
            // Email logic would go here
            return RedirectToAction("Index");
        }
    }
}

