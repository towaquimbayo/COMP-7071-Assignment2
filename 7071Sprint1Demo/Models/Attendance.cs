
using System;
using System.ComponentModel.DataAnnotations;

namespace _7071Sprint1Demo.Models

{
    public class Attendance
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid ShiftId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Shift Shift { get; set; }

        public bool ApprovedByManager { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }
    }
}