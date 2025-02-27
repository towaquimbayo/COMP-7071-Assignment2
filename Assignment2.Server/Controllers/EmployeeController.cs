using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Assignment2.Server.Data;
using Assignment2.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Server.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // List all employees.
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.Manager).ToList();
            return View(employees);
        }

        // View employee details.
        public ActionResult Details(Guid id)
        {
            var employee = db.Employees.Include(e => e.PayrollHistory)
                                       .Include(e => e.Shifts)
                                       .FirstOrDefault(e => e.Id == id);
            if (employee == null)
                return HttpNotFound();
            return View(employee);
        }
        // Create new employee.
        public ActionResult Create()
        {
            ViewBag.Managers = db.Employees.ToList();
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
            ViewBag.Managers = db.Employees.ToList();
            return View(employee);
        }

        // Edit existing employee.
        public ActionResult Edit(Guid id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null)
                return HttpNotFound();
            ViewBag.Managers = db.Employees.ToList();
            return View(employee);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Managers = db.Employees.ToList();
            return View(employee);
        }
    }
}