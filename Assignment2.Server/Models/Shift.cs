namespace Assignment2.Server.Models
{
    public class Shift
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
        public bool IsRecurring { get; set; }

        public List<Attendance> Attendances { get; set; }
    }
}
