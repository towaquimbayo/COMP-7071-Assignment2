using System.ComponentModel.DataAnnotations;

namespace Assignment2.Server.Models
{
    public enum RequestStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class VacationRequest
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public RequestStatus Status { get; set; }
        public bool ApprovedByManager { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}
