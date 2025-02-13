namespace Assignment2.Server.Models
{
    public class RentInvoice
    {
        private Guid Id { get; set; }
        private Guid AssetId { get; set; }
        private Guid RenterId { get; set; }
        private DateTime IssueDate { get; set; }
        private DateTime DueDate { get; set; }
        private decimal AmountDue { get; set; }
        private bool IsPaid { get; set; }
        private Asset Asset { get; set; }
        private Renter Renter { get; set; }
    }
}
