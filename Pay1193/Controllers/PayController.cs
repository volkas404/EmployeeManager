using Microsoft.AspNetCore.Mvc;
using Pay1193.Entity;
using Pay1193.Models;
using Pay1193.Services;
using Pay1193.Services.Implement;
using System.IO.Pipes;

namespace Pay1193.Controllers
{
    public class PayController : Controller
    {
        private readonly IPayService _payService;
        private readonly ITaxService _taxService;
        private readonly INationalInsuranceService _nationalInsuranceService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PayController(IPayService payService, ITaxService taxService, INationalInsuranceService nationalInsuranceService, IWebHostEnvironment webHostEnvironment)
        {
            _payService = payService;
            _taxService = taxService;
            _nationalInsuranceService = nationalInsuranceService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var pay = _payService.GetAll().Select(pay => new PayIndexViewModel
            {
                Id = pay.Id,
                EmployeeId = pay.EmployeeId,
                ContractualEarnings = pay.ContractualEarnings,
                OvertimeEarnings = pay.OvertimeEarnings,
                NiC = pay.NiC,
                Tax = pay.Tax,
                SLC = pay.SLC,
                TotalEarnings = pay.TotalEarnings,
                EarningDeduction = pay.EarningDeduction,
                UnionFee = pay.UnionFee,
                DatePay = pay.DatePay,
                NetPayment = pay.NetPayment
            }).ToList();
            return View(pay);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PayCreateViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PayCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var overtimehours = _payService.OverTimeHours(model.HourWorked, model.ContractualHours);
                var overtimerate = _payService.OvertimeRate(model.HourlyRate);
                var contractualearning = _payService.ContractualEarnings(model.ContractualHours,
                    model.HourWorked, model.HourlyRate);
                var overtimeearning = _payService.OvertimeEarnings(overtimehours, overtimerate);
                var total = _payService.TotalEarning(contractualearning, overtimeearning);
                var tax = _taxService.TaxAmount(total);
                var nic = _nationalInsuranceService.NIContribution(total);
                var deduction = _payService.TotalDeduction(tax, nic, model.SLC, model.UnionFee);
                var netpay = _payService.NetPay(total, deduction);
                var pay = new PaymentRecord
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    TaxYearId = 1,
                    TaxCode = model.TaxCode,
                    DatePay = model.DatePay,
                    MonthPay = model.DatePay,
                    HourlyRate = model.HourlyRate,
                    HourWorked = model.HourWorked,
                    OvertimeHours = overtimehours,
                    ContractualHours = model.ContractualHours,
                    ContractualEarnings = contractualearning,
                    OvertimeEarnings = overtimeearning,
                    NiC = nic,
                    Tax = tax,
                    UnionFee = model.UnionFee,
                    SLC = model.SLC,
                    TotalEarnings = total,
                    EarningDeduction = deduction,
                    NetPayment =netpay,
                };
                await _payService.CreateAsync(pay);
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var pay = _payService.GetById(id);
            if (pay == null)
            {
                return NotFound();
            }
            var model = new PayDeleteViewModel
            {
                Id = pay.Id,            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(PayDeleteViewModel model)
        {
            await _payService.Delete(model.Id);
            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            var pay = _payService.GetById(id);
            if (pay == null)
            {
                return NotFound();
            }
            PayDetailsViewModel model = new PayDetailsViewModel()
            {
                Id = pay.Id,
                EmployeeId= pay.EmployeeId,
                TaxCode=pay.TaxCode,
                DatePay=pay.DatePay,
                ContractualEarnings=pay.ContractualEarnings,
                OvertimeEarnings=pay.OvertimeEarnings,
                NiC=pay.NiC,
                Tax=pay.Tax,
                UnionFee=pay.UnionFee,
                SLC=pay.SLC,
                TotalEarnings=pay.TotalEarnings,
                EarningDeduction=pay.EarningDeduction,
                NetPayment=pay.NetPayment,

            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var pay = _payService.GetById(id);
            if (pay == null)
            {
                return NotFound();
            }
            var overtimehours = _payService.OverTimeHours(pay.HourWorked, pay.ContractualHours);
            var overtimerate = _payService.OvertimeRate(pay.HourlyRate);
            var contractualearning = _payService.ContractualEarnings(pay.ContractualHours,
                pay.HourWorked, pay.HourlyRate);
            var overtimeearning = _payService.OvertimeEarnings(overtimehours, overtimerate);
            var total = _payService.TotalEarning(contractualearning, overtimeearning);
            var tax = _taxService.TaxAmount(total);
            var nic = _nationalInsuranceService.NIContribution(total);
            var deduction = _payService.TotalDeduction(tax, nic, (decimal)pay.SLC, pay.UnionFee);
            var netpay = _payService.NetPay(total, deduction);
            var model = new PayEditViewModel()
            {
                Id = pay.Id,
                EmployeeId = pay.EmployeeId,
                TaxCode = pay.TaxCode,
                DatePay = pay.DatePay,
                HourlyRate = pay.HourlyRate,
                HourWorked = pay.HourWorked,
                OvertimeHours = overtimehours,
                ContractualHours = pay.ContractualHours,
                ContractualEarnings = contractualearning,
                OvertimeEarnings = overtimeearning,
                NiC = nic,
                Tax = tax,
                UnionFee = pay.UnionFee,
                SLC = (decimal)pay.SLC,
                TotalEarnings = total,
                EarningDeduction = deduction,
                NetPayment = netpay,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PayEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var overtimehours = _payService.OverTimeHours(model.HourWorked, model.ContractualHours);
                var overtimerate = _payService.OvertimeRate(model.HourlyRate);
                var contractualearning = _payService.ContractualEarnings(model.ContractualHours,
                    model.HourWorked, model.HourlyRate);
                var overtimeearning = _payService.OvertimeEarnings(overtimehours, overtimerate);
                var total = _payService.TotalEarning(contractualearning, overtimeearning);
                var tax = _taxService.TaxAmount(total);
                var nic = _nationalInsuranceService.NIContribution(total);
                var deduction = _payService.TotalDeduction(tax, nic, model.SLC, model.UnionFee);
                var netpay = _payService.NetPay(total, deduction);
                var pay = new PaymentRecord
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    TaxYearId = 1,
                    TaxCode = model.TaxCode,
                    DatePay = model.DatePay,
                    MonthPay = model.DatePay,
                    HourlyRate = model.HourlyRate,
                    HourWorked = model.HourWorked,
                    OvertimeHours = overtimehours,
                    ContractualHours = model.ContractualHours,
                    ContractualEarnings = contractualearning,
                    OvertimeEarnings = overtimeearning,
                    NiC = nic,
                    Tax = tax,
                    UnionFee = model.UnionFee,
                    SLC = model.SLC,
                    TotalEarnings = total,
                    EarningDeduction = deduction,
                    NetPayment = netpay,

                };
                await _payService.UpdateAsync(pay);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
