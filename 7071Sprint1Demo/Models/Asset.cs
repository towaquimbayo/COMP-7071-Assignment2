using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace _7071Sprint1Demo.Models
{
 
    public class Asset
        {
            [Key]
            public Guid Id { get; set; }

            [Required, StringLength(50)]
            public string Type { get; set; }

            [Range(0, double.MaxValue)]
            public decimal RentAmount { get; set; }

            public bool IsOccupied { get; set; }

            public virtual List<OccupancyHistory> OccupancyHistory { get; set; } = new List<OccupancyHistory>();
            public virtual List<RentHistory> RentHistory { get; set; } = new List<RentHistory>();
            public virtual List<RentInvoice> RentInvoices { get; set; } = new List<RentInvoice>();
        }
}


