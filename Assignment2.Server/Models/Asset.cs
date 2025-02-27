namespace Assignment2.Server.Models
{
    public class Asset
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public decimal RentAmount { get; set; }
        public bool IsOccupied { get; set; }

        public List<OccupancyHistory> OccupancyHistory { get; set; }

        public List<RentHistory> RentHistory { get; set; }

        public List<RentInvoice> RentInvoices { get; set; }
    }
}
