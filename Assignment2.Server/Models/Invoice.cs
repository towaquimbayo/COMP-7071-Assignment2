namespace Assignment2.Server.Models
{
    public class Invoice
    {
        private Guid Id { get; set; }
        private Guid ClientId { get; set; }
        private Client Client { get; set; }
        private DateTime IssueDate { get; set; }
        private decimal TotalAmount { get; set; }
        private bool IsPaid { get; set; }
    }
}
