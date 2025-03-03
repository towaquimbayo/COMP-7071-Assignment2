using System.ComponentModel.DataAnnotations;

namespace _7071Sprint1Demo.Models
{



    public class Service
{
    [Key]
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Rate { get; set; }

    public virtual List<ServiceBooking> Bookings { get; set; } = new List<ServiceBooking>();
}
}