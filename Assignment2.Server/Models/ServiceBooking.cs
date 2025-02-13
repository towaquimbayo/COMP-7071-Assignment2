namespace Assignment2.Server.Models
{
    public class ServiceBooking
    {
        private Guid Id { get; set; }
        private Guid ClientId { get; set; }
        private Client Client { get; set; }
        private Guid ServiceId { get; set; }
        private Service Service { get; set; }
        private DateTime ScheduledDate { get; set; }
        private List<Employee> AssignedEmployees { get; set; }
    }
}
