namespace Assignment2.Server.Models
{
    public class OccupancyHistory
    {
        public Guid Id { get; set; }
        public Guid AssetId { get; set; }
        public Guid RenterId { get; set; }

        public Asset Asset { get; set; }

        public Renter Renter { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
