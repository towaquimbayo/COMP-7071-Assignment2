using System.ComponentModel.DataAnnotations;

namespace _7071Sprint1Demo.Models
{
    public class VacationRequest
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public bool ApprovedByManager { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}
