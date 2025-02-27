namespace Assignment2.Server.Models
{
    public class Payroll
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public DateTime PayDate { get; set; }
        public decimal BasePay { get; set; }
        public decimal Deductions { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal TaxRate { get; set; }
        public decimal NetPay { get; set; }
    }
}
