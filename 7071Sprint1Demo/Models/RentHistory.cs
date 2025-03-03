using System.ComponentModel.DataAnnotations;

namespace _7071Sprint1Demo.Models
{
    public class RentHistory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid AssetId { get; set; }

        [Required]
        public Guid RenterId { get; set; }

        public virtual Asset Asset { get; set; }
        public virtual Renter Renter { get; set; }

        [Range(0, double.MaxValue)]
        public decimal OldRentAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal NewRentAmount { get; set; }

        public DateTime EffectiveDate { get; set; }
    }
}