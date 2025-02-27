namespace Assignment2.Server.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }

        public List<ServiceBooking> Bookings { get; set; }

        public List<Invoice> Invoices { get; set; }
    }
}
