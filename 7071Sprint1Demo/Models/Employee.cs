using _7071Sprint1Demo.Migrations;
using _7071Sprint1Demo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace _7071Sprint1Demo.Models
{

    public enum EmploymentType { FullTime, PartTime, OnCall }
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, Phone]
        [Display(Name = "Emergency Contact")]
        public string EmergencyContact { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Range(0, double.MaxValue)]
        [Display(Name = "Pay Rate")]
        [DataType(DataType.Currency)]
        public decimal PayRate { get; set; }

        [Required, EmailAddress, StringLength(150)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public virtual List<Payroll> PayrollHistory { get; set; } = new List<Payroll>();
        public virtual List<Shift> Shifts { get; set; } = new List<Shift>();
        public virtual List<Attendance> Attendances { get; set; } = new List<Attendance>();

        [Display(Name = "Manager")]
        public Guid? ManagerId { get; set; }

        [ForeignKey("ManagerId")]
        [Display(Name = "Manager")]
        public virtual Employee? Manager { get; set; }

        public EmploymentType? EmploymentType { get; set; }

        public virtual List<ServiceBooking> ServiceBookings { get; set; } = new List<ServiceBooking>();
        public virtual List<VacationRequest> VacationRequests { get; set; } = new List<VacationRequest>();

        public override string ToString()
        {
            return Name;
        }
    }
}
