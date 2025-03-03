using System;
using System.ComponentModel.DataAnnotations;
namespace _7071Sprint1Demo.Models
{
    public class ServiceBooking
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public Guid ServiceId { get; set; }

        public Client Client { get; set; }
        public virtual Service Service { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ScheduledDate { get; set; }

        public virtual List<Employee> AssignedEmployees { get; set; } = new List<Employee>();
    }
}