namespace Assignment2.Server.Models
{
    public class Attendance
    {
        private Guid Id { get; set; }
        private Guid EmployeeId { get; set; }
        private Guid ShiftId { get; set; }
        private Employee Employee { get; set; }
        private Shift Shift { get; set; }
        private bool ApprovedByManager { get; set; }
        private string Notes { get; set; }
    }
}
