namespace Assignment2.Server.Models
{
    public enum EmploymentType
    {
        FullTime,
        PartTime,
        OnCall
    }

    public class Employee
    {
        private Guid Id { get; set; }
        private string Name { get; set; }
        private string Address { get; set; }
        private string EmergencyContact { get; set; }
        private string JobTitle { get; set; }
        private EmploymentType EmploymentType { get; set; }
        private decimal PayRate { get; set; } // Salary or hourly rate
        private List<Payroll> PayrollHistory { get; set; }
        private List<Shift> Shifts { get; set; }
        private Employee? Manager { get; set; }
    }
}
