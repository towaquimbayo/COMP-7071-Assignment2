using System;
using _7071Sprint1Demo.Models;

namespace _7071Sprint1Demo.Services
{
    public static class EmailService
    {
        public static void NotifyAttendanceChange(Attendance attendance)
        {
            // In production, integrate with SMTP or a third-party API.
            Console.WriteLine($"[Email] Attendance update for Employee {attendance.EmployeeId} on Shift {attendance.ShiftId}.");
        }

        public static void SendInvoice(Invoice invoice)
        {
            Console.WriteLine($"[Email] Invoice {invoice.Id} sent to Client {invoice.ClientId}.");
        }

        public static void SendRentInvoice(RentInvoice invoice)
        {
            Console.WriteLine($"[Email] Rent Invoice {invoice.Id} sent to Renter {invoice.RenterId}.");
        }
    }
}