using System;
using System.Threading.Tasks;

namespace EmailNotifier
{
    public interface IEmailNotifier 
    { 
        Task SendInvoiceNotificationAsync(Guid invoiceId, string recipientEmail, string invoiceDetails); 
        Task SendRentInvoiceNotificationAsync(Guid rentInvoiceId, string recipientEmail, string rentInvoiceDetails); 
        Task SendAttendanceNotificationAsync(Guid attendanceId, string recipientEmail, string attendanceDetails); 
    }
}
