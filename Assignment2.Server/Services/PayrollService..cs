using System;
using System.Linq;
using Assignment2.Server.Data;
using Assignment2.Server.Models;

namespace Assignment2.Server.Services
{
    public static class PayrollService
    {
        public static void GeneratePayroll()
        {
            using (var db = new ApplicationDbContext())
            {
                var employees = db.Employees.ToList();
                foreach (var emp in employees)
                {
                    // Simplified payroll calculation – replace with real logic.
                    var basePay = emp.PayRate * 8; // Assume one 8-hour shift.
                    decimal deductions = 0;
                    decimal overtime = 0;
                    decimal taxRate = 0.15m;
                    decimal netPay = (basePay + overtime - deductions) * (1 - taxRate);

                    var payroll = new Payroll
                    {
                        Id = Guid.NewGuid(),
                        EmployeeId = emp.Id,
                        PayDate = DateTime.Now,
                        BasePay = basePay,
                        Deductions = deductions,
                        OvertimePay = overtime,
                        TaxRate = taxRate,
                        NetPay = netPay
                    };

                    db.Payrolls.Add(payroll);
                }
                db.SaveChanges();
            }
        }
    }
}