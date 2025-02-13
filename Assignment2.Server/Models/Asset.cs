namespace Assignment2.Server.Models
{
    public class Asset
    {
        private Guid Id { get; set; }
        private string Type { get; set; }
        private decimal RentAmount { get; set; }
        private bool IsOccupied { get; set; }
        private List<OccupancyHistory> OccupancyHistory { get; set; }
    }
}
