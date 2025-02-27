namespace Assignment2.Server.Models
{
    public class Renter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public string EmergencyContact { get; set; }

        public List<OccupancyHistory> OccupancyHistories { get; set; }

        public List<RentHistory> RentHistories { get; set; }

        public List<RentInvoice> RentInvoices { get; set; }
    }
}
