namespace Assignment2.Server.Models
{
    public class RentInvoice
    {
        public Guid Id { get; set; }
        public Guid AssetId { get; set; }
        public Guid RenterId { get; set; }

        public Asset Asset { get; set; }

        public Renter Renter { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal AmountDue { get; set; }
        public bool IsPaid { get; set; }
    }
}
