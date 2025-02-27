using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Assignment2.Server.Data;
using Assignment2.Server.Models;
using Assignment2.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Server.Controllers
{
    public class PayrollController(ApplicationDbContext db, PayrollService payrollService) : Controller
    {
        private readonly ApplicationDbContext _db = db;
        private readonly PayrollService _payrollService = payrollService;

        // List payroll records.
        public ActionResult Index()
        {
            var payrolls = _db.Payrolls.Include(p => p.Employee).ToList();
            return View(payrolls);
        }

        // View payroll details.
        public ActionResult Details(Guid id)
        {
            var payroll = _db.Payrolls.Find(id);
            if (payroll == null)
                return NotFound();
            return View(payroll);
        }

        // Generate payroll for all employees.
        public ActionResult GeneratePayroll()
        {
            _payrollService.GeneratePayroll();
            return RedirectToAction("Index");
        }
    }
}