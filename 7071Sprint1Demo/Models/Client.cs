using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace _7071Sprint1Demo.Models
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string ContactInfo { get; set; }

        public virtual List<ServiceBooking> Bookings { get; set; } = new List<ServiceBooking>();
        public virtual List<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
