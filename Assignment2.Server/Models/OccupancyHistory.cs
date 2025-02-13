namespace Assignment2.Server.Models
{
    public class OccupancyHistory
    {
        private Guid id { get; set; }
        private Guid AssetId { get; set; }
        private Guid RenterId { get; set; }
        private DateTime StartDate { get; set; }
        private DateTime? EndDate { get; set; }
        private Asset Asset { get; set; }
        private Renter Renter { get; set; }
    }
}
