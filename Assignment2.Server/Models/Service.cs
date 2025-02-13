namespace Assignment2.Server.Models
{
    public class Service
    {
        private Guid Id { get; set; }
        private string Name { get; set; }
        private string Description { get; set; }
        private decimal Rate { get; set; }
        private List<ServiceBooking> Bookings { get; set; }
    }
}
