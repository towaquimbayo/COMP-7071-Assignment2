namespace Assignment2.Server.Models
{
    public class Payroll
    {
        private Guid Id { get; set; }
        private Guid EmployeeId { get; set; }
        private Employee Employee { get; set; }
        private DateTime PayDate { get; set; }
        private decimal BasePay { get; set; }
        private decimal Deductions { get; set; }
        private decimal OvertimePay { get; set; }
        private decimal TaxRate { get; set; }
        private decimal NetPay { get; set; }
    }
}
