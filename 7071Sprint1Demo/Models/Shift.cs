using System.ComponentModel.DataAnnotations;

namespace _7071Sprint1Demo.Models
{
    public class Shift
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }

        public bool IsRecurring { get; set; }

        public virtual List<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}