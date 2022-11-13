using Pay1193.Entity;
using Pay1193.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pay1193.Services.Implement
{
    public class PayService : IPayService
    {
        private decimal contractualEarnings;
        private readonly ApplicationDbContext _context;
        public PayService(ApplicationDbContext context)
        {
            _context = context;
        }
        public decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
        {
            if(hoursWorked < contractualHours)
            {
                contractualEarnings = hoursWorked * hourlyRate;

            }
            else
            {
                contractualEarnings = contractualHours * hourlyRate;
            }
            return contractualEarnings;
        }

        public async Task CreateAsync(PaymentRecord paymentRecord)
        {
            await _context.PaymentRecords.AddAsync(paymentRecord);
            await _context.SaveChangesAsync();
        }

        public Task UpdateAsync(PaymentRecord paymentRecord)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PaymentRecord> GetAll()
        {
            return _context.PaymentRecords.ToList();
        }

        public PaymentRecord GetById(int id)
        {
            return _context.PaymentRecords.Where(u => u.Id == id).FirstOrDefault();
        }

        public async Task Delete(int id)
        {
            var pay = GetById(id);
            _context.PaymentRecords.Remove(pay);
            await _context.SaveChangesAsync();
        }

        public TaxYear GetTaxYearById(int id)
        {
            throw new NotImplementedException();
        }

        public decimal NetPay(decimal totalEarnings, decimal totalDeduction)
        {
            return totalEarnings - totalDeduction;
        }

        public decimal OvertimeEarnings(decimal overtimeHours, decimal overtimeRate)
        {
            return overtimeHours * overtimeRate;
        }

        public decimal OverTimeHours(decimal hoursWorked, decimal contractualHours)
        {
            if (hoursWorked < contractualHours)
            {
                return 0;

            }
            else
            {
                return hoursWorked - contractualHours;
            }
        }

        public decimal OvertimeRate(decimal hourlyRate)
        {
            return hourlyRate * 2;
        }

        public decimal TotalDeduction(decimal tax, decimal nic, decimal studentLoanRepayment, decimal unionFees)
        {
            return tax + nic + studentLoanRepayment + unionFees;
        }
        public decimal TotalEarning(decimal contractualEarnings, decimal overtimeEarnings)
        {
            return contractualEarnings + overtimeEarnings;
        }
    }
}
