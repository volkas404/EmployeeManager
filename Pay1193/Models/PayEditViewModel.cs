using Pay1193.Entity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Pay1193.Models
{
    public class PayEditViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Employee Number is required")]
        [RegularExpression(@"^[0-9]$")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "TaxCode is required"), StringLength(50, MinimumLength = 2)]
        public string TaxCode { get; set; }
        [DataType(DataType.Date), Display(Name = "DatePay")]
        public DateTime DatePay { get; set; }
        [Required(ErrorMessage = "HourlyRate is required")]
        public Decimal HourlyRate { get; set; }
        [Required(ErrorMessage = "HourWorked is required")]
        public Decimal HourWorked { get; set; }
        [Required(ErrorMessage = "ContractualHours is required")]
        public Decimal ContractualHours { get; set; }
        public Decimal OvertimeHours { get; set; }
        public Decimal ContractualEarnings { get; set; }
        public Decimal OvertimeEarnings { get; set; }
        public Decimal NiC { get; set; }
        public Decimal Tax { get; set; }
        public Decimal UnionFee { get; set; }
        public Decimal SLC { get; set; }
        public Decimal TotalEarnings { get; set; }
        public Decimal EarningDeduction { get; set; }
        public Decimal NetPayment { get; set; }
    }
}
