namespace Assignment2.Server.Models
{
    public class RentHistory
    {
        public Guid Id { get; set; }
        public Guid AssetId { get; set; }
        public Guid RenterId { get; set; }

        public Asset Asset { get; set; }

        public Renter Renter { get; set; }

        public decimal OldRentAmount { get; set; }
        public decimal NewRentAmount { get; set; }
        public DateTime EffectiveDate { get; set; }
    }
}
