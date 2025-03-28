using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmailNotifier
{
    public class EmailNotifierService : IEmailNotifier
    {
        private readonly EmailSettings _settings;

        public EmailNotifierService(EmailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendInvoiceNotificationAsync(Guid invoiceId, string recipientEmail, string invoiceDetails)
        {
            var subject = $"Invoice Notification - {invoiceId}";
            var bodyText = $"Your invoice (ID: {invoiceId}) has been generated.\nDetails: {invoiceDetails}";
            var message = CreateMessage(subject, bodyText, recipientEmail);
            await SendEmailAsync(message);
        }

        public async Task SendRentInvoiceNotificationAsync(Guid rentInvoiceId, string recipientEmail, string rentInvoiceDetails)
        {
            var subject = $"Rent Invoice Notification - {rentInvoiceId}";
            var bodyText = $"Your rent invoice (ID: {rentInvoiceId}) has been generated.\nDetails: {rentInvoiceDetails}";
            var message = CreateMessage(subject, bodyText, recipientEmail);
            await SendEmailAsync(message);
        }

        public async Task SendAttendanceNotificationAsync(Guid attendanceId, string recipientEmail, string attendanceDetails)
        {
            var subject = $"Attendance Notification - {attendanceId}";
            var bodyText = $"Your attendance record (ID: {attendanceId}) has been updated.\nDetails: {attendanceDetails}";
            var message = CreateMessage(subject, bodyText, recipientEmail);
            await SendEmailAsync(message);
        }

        private MimeMessage CreateMessage(string subject, string bodyText, string recipientEmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress(recipientEmail, recipientEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = bodyText
            };

            return message;
        }

        private async Task SendEmailAsync(MimeMessage message)
        {
            try
            {
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(
                        _settings.SmtpServer,
                        _settings.SmtpPort,
                        MailKit.Security.SecureSocketOptions.StartTls
                    );

                    await client.AuthenticateAsync(_settings.Username, _settings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

    }
}