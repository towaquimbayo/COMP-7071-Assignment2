using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Assignment2.Server.Data;
using Assignment2.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Server.Controllers
{
    public class EmployeesController(ApplicationDbContext db) : Controller
    {
        private readonly ApplicationDbContext _db = db;

        // List all employees.
        public ActionResult Index()
        {
            var employees = _db.Employees.Include(e => e.Manager).ToList();
            return View(employees);
        }

        // View employee details.
        public ActionResult Details(Guid id)
        {
            var employee = _db.Employees.Include(e => e.PayrollHistory)
                                       .Include(e => e.Shifts)
                                       .FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return NotFound();
            return View(employee);
        }
        // Create new employee.
        public ActionResult Create()
        {
            ViewBag.Managers = _db.Employees.ToList();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Id = Guid.NewGuid();
                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Managers = _db.Employees.ToList();
            return View(employee);
        }

        // Edit existing employee.
        public ActionResult Edit(Guid id)
        {
            var employee = _db.Employees.Find(id);
            if (employee == null)
                return NotFound();
            ViewBag.Managers = _db.Employees.ToList();
            return View(employee);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(employee).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Managers = _db.Employees.ToList();
            return View(employee);
        }
    }
}