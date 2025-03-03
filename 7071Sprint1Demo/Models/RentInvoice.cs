using System.ComponentModel.DataAnnotations;

namespace _7071Sprint1Demo.Models
{
    public class RentInvoice
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid AssetId { get; set; }

        [Required]
        public Guid RenterId { get; set; }

        public virtual Asset Asset { get; set; }
        public virtual Renter Renter { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal AmountDue { get; set; }

        public bool IsPaid { get; set; }
    }
}