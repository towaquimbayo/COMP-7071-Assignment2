namespace Assignment2.Server.Models
{
    public class ServiceBooking
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public Guid ServiceId { get; set; }

        public Client Client { get; set; }

        public Service Service { get; set; }

        public DateTime ScheduledDate { get; set; }

        public List<Employee> AssignedEmployees { get; set; }
    }
}
