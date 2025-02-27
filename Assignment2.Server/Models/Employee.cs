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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string EmergencyContact { get; set; }
        public string JobTitle { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public decimal PayRate { get; set; }

        public List<Payroll> PayrollHistory { get; set; }

        public List<Shift> Shifts { get; set; }

        public List<Attendance> Attendances { get; set; }

        public Guid? ManagerId { get; set; }
        public Employee Manager { get; set; }

        public List<ServiceBooking> ServiceBookings { get; set; }

        public List<VacationRequest> VacationRequests { get; set; }
    }
}
