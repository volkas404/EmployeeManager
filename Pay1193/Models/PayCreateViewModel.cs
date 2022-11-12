using Pay1193.Entity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Pay1193.Models
{
    public class PayCreateViewModel
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
        [Required(ErrorMessage = "OvertimeHours is required")]
        public Decimal SLC { get; set; }
        public Decimal UnionFee { get; set; }
    }
}
