using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _7071Sprint1Demo.Data;
using _7071Sprint1Demo.Models;
using EmailNotifier;
using Microsoft.AspNetCore.Mvc.Rendering;
using _7071Sprint1Demo.Services;


namespace _7071Sprint1Demo.Controllers
{
    public class RentInvoiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailNotifier _emailNotifier;

        public RentInvoiceController(ApplicationDbContext context, IEmailNotifier emailNotifier)
        {
            _context = context;
            _emailNotifier = emailNotifier;
        }

        // GET: RentInvoice
        public async Task<IActionResult> Index()
        {
            var invoices = await _context.RentInvoices
                                          .Include(i => i.Asset)
                                          .Include(i => i.Renter)
                                          .ToListAsync();
            return View(invoices);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var invoice = await _context.RentInvoices
                                        .Include(i => i.Asset)
                                        .Include(i => i.Renter)
                                        .FirstOrDefaultAsync(i => i.Id == id);
            if (invoice == null)
                return NotFound();

            return View(invoice);
        }

        // GET: RentInvoice/Create
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
                try
                {
                    rentInvoice.Id = Guid.NewGuid();
                    _context.Add(rentInvoice);
                    await _context.SaveChangesAsync();

                    try
                    {
                        var renter = await _context.Renters.FindAsync(rentInvoice.RenterId);
                        var details = $"Rent invoice created - Amount: {rentInvoice.AmountDue}, Due: {rentInvoice.DueDate}";
                        await _emailNotifier.SendRentInvoiceNotificationAsync(rentInvoice.Id, renter.ContactInfo, details);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending rent invoice email: {ex.Message}");
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating invoice: {ex.Message}");
                }
            }

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
                try
                {
                    // Update the existing invoice record without generating a new Id.
                    _context.Update(rentInvoice);
                    await _context.SaveChangesAsync();

                    try
                    {
                        var renter = await _context.Renters.FindAsync(rentInvoice.RenterId);
                        var details = $"Updated rent invoice - Amount: {rentInvoice.AmountDue}, Due: {rentInvoice.DueDate}";
                        await _emailNotifier.SendRentInvoiceNotificationAsync(rentInvoice.Id, renter.ContactInfo, details);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending rent invoice update email: {ex.Message}");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentInvoiceExists(rentInvoice.Id))
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
        public async Task<IActionResult> DeleteConfirm(Guid id)
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

        private bool RentInvoiceExists(Guid id)
        {
            return _context.RentInvoices.Any(e => e.Id == id);
        }
    }
}


