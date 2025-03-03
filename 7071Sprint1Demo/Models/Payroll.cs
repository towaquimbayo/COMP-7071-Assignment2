using System.ComponentModel.DataAnnotations;

namespace _7071Sprint1Demo.Models
{
    public class Payroll
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public DateTime PayDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal BasePay { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Deductions { get; set; }

        [Range(0, double.MaxValue)]
        public decimal OvertimePay { get; set; }

        [Range(0, 1)]
        public decimal TaxRate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal NetPay { get; set; }
    }
}