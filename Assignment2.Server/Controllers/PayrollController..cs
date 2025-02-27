using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Assignment2.Server.Data;
using Assignment2.Server.Models;
using Assignment2.Server.Services;

namespace Assignment2.Server.Controllers
{
    public class PayrollController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // List payroll records.
        public ActionResult Index()
        {
            var payrolls = db.Payrolls.Include(p => p.Employee).ToList();
            return View(payrolls);
        }

        // View payroll details.
        public ActionResult Details(Guid id)
        {
            var payroll = db.Payrolls.Find(id);
            if (payroll == null)
                return HttpNotFound();
            return View(payroll);
        }

        // Generate payroll for all employees.
        public ActionResult GeneratePayroll()
        {
            PayrollService.GeneratePayroll();
            return RedirectToAction("Index");
        }
    }
}