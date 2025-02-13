namespace Assignment2.Server.Models
{
    public class Shift
    {
        private Guid Id { get; set; }
        private Guid EmployeeId { get; set; }
        private Employee Employee { get; set; }
        private DateTime ShiftStart { get; set; }
        private DateTime ShiftEnd { get; set; }
        private bool IsRecurring { get; set; }
    }
}
