namespace Assignment2.Server.Models
{
    public class RentHistory
    {
        private Guid id { get; set; }
        private Guid AssetId { get; set; }
        private Guid RenderId { get; set; }
        private decimal OldRentAmount { get; set; }
        private decimal NewRentAmount { get; set; }
        private DateTime EffectiveDate { get; set; }
    }
}
