namespace Assignment2.Server.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }

        public List<ServiceBooking> Bookings { get; set; }
    }
}
