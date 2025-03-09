using _7071Sprint1Demo.Data;
using _7071Sprint1Demo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _7071Sprint1Demo.Services;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace _7071Sprint1Demo.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Invoices.Include(i => i.Client);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClientId,IssueDate,TotalAmount,IsPaid")] Invoice invoice)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Generate new ID
                    invoice.Id = Guid.NewGuid();

                    // Save to database
                    _context.Add(invoice);
                    await _context.SaveChangesAsync();

                    try
                    {
                        // Try to send email, but continue even if it fails
                        EmailService.SendInvoice(invoice);
                    }
                    catch (Exception ex)
                    {
                        // Log the error but don't stop the process
                        Console.WriteLine($"Error sending email: {ex.Message}");
                        // Optionally add to ModelState or TempData if you want to show a message to the user
                        // TempData["EmailError"] = "Invoice was created but email notification failed.";
                    }
                    try
                    {
                        // Enhanced logging for email service testing
                        Console.WriteLine("-----BEGIN EMAIL SERVICE TEST [Create]-----");
                        Console.WriteLine($"Timestamp: {DateTime.Now}");
                        Console.WriteLine($"Attempting to email invoice {invoice.Id} to client {invoice.ClientId}");

                        EmailService.SendInvoice(invoice);

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
                catch (Exception ex)
                {
                    // Handle any general exceptions
                    ModelState.AddModelError("", $"Error creating invoice: {ex.Message}");
                }
            }

            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", invoice.ClientId);
            return View(invoice);
        }



        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", invoice.ClientId);
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ClientId,IssueDate,TotalAmount,IsPaid")] Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();

                    try
                    {
                        // Enhanced logging for email service testing
                        Console.WriteLine("-----BEGIN EMAIL SERVICE TEST [Edit]-----");
                        Console.WriteLine($"Timestamp: {DateTime.Now}");
                        Console.WriteLine($"Attempting to email updated invoice {invoice.Id} to client {invoice.ClientId}");

                        EmailService.SendInvoice(invoice);

                        Console.WriteLine("Email service call completed successfully");
                        Console.WriteLine("-----END EMAIL SERVICE TEST [Edit]-----");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"-----EMAIL SERVICE ERROR [Edit]-----");
                        Console.WriteLine($"Timestamp: {DateTime.Now}");
                        Console.WriteLine($"Error sending email: {ex.Message}");
                        Console.WriteLine($"Stack trace: {ex.StackTrace}");
                        Console.WriteLine($"-----END EMAIL SERVICE ERROR [Edit]-----");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "Id", "Name", invoice.ClientId);
            return View(invoice);
       
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoices
                .Include(i => i.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(Guid id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}