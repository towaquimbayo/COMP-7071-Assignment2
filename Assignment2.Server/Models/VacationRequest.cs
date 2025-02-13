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
        private Guid RequestId { get; set; }
        private Guid EmployeeId { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime EndDate { get; set; }
        private RequestStatus Status { get; set; }
        private bool ApprovedByManager { get; set; }
        private DateTime? ApprovalDate { get; set; }
    }
}
