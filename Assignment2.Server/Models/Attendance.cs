namespace Assignment2.Server.Models
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ShiftId { get; set; }

        public Employee Employee { get; set; }

        public Shift Shift { get; set; }

        public bool ApprovedByManager { get; set; }
        public string Notes { get; set; }
    }
}
