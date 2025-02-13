namespace Assignment2.Server.Models
{
    public class Client
    {
        private Guid Id { get; set; }
        private string Name { get; set; }
        private string ContactInfo { get; set; }
        private List<ServiceBooking> Bookings { get; set; }
    }
}
