namespace EmailNotifier
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = "smtp.example.com";
        public int SmtpPort { get; set; } = 587;
        public bool UseSsl { get; set; } = true;
        public string Username { get; set; } = "your_username";
        public string Password { get; set; } = "your_password";
        public string FromEmail { get; set; } = "no-reply@example.com";
        public string FromName { get; set; } = "Your Company";
    }
}
