using System.ComponentModel.DataAnnotations;

namespace _7071Sprint1Demo.Models
{
    public class OccupancyHistory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid AssetId { get; set; }

        [Required]
        public Guid RenterId { get; set; }

        public virtual Asset Asset { get; set; }
        public virtual Renter Renter { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}