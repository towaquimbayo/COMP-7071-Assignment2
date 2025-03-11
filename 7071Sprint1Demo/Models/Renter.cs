using System.ComponentModel.DataAnnotations;

namespace _7071Sprint1Demo.Models
{
    public class Renter
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string ContactInfo { get; set; }

        [StringLength(200)]
        public string EmergencyContact { get; set; }

        public virtual List<OccupancyHistory> OccupancyHistories { get; set; } = new List<OccupancyHistory>();
        public virtual List<RentHistory> RentHistories { get; set; } = new List<RentHistory>();
        public virtual List<RentInvoice> RentInvoices { get; set; } = new List<RentInvoice>();

       
    }

}