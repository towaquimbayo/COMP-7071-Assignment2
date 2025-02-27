namespace Assignment2.Server.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public DateTime IssueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }
    }
}
