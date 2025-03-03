using System;
using System.ComponentModel.DataAnnotations;

namespace _7071Sprint1Demo.Models
{
    public class Invoice
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }

        public virtual Client Client { get; set; }

        public DateTime IssueDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        public bool IsPaid { get; set; }
    }
}
