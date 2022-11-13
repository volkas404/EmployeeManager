using Pay1193.Entity;
using System.ComponentModel.DataAnnotations;

namespace Pay1193.Models
{
    public class PayDetailsViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string TaxCode { get; set; }
        public Decimal ContractualEarnings { get; set; }
        public Decimal OvertimeEarnings { get; set; }
        public DateTime DatePay { get; set; }
        public Decimal NiC { get; set; }
        public Decimal Tax { get; set; }
        public Decimal UnionFee { get; set; }
        public Nullable<decimal> SLC { get; set; }
        public Decimal TotalEarnings { get; set; }
        public Decimal EarningDeduction { get; set; }
        public Decimal NetPayment { get; set; }
    }
}
