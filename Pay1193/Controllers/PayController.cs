using Microsoft.AspNetCore.Mvc;
using Pay1193.Entity;
using Pay1193.Models;
using Pay1193.Services;
using Pay1193.Services.Implement;

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
                NiC = pay.NiC,
                Tax = pay.Tax,
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
                var pay = new PaymentRecord
                {
                    Id = model.Id,
                    EmployeeId = model.EmployeeId,
                    TaxCode = model.TaxCode,
                    DatePay = model.DatePay,
                    MonthPay = model.DatePay,
                    HourlyRate = model.HourlyRate,
                    HourWorked = model.HourWorked,
                    ContractualHours = model.ContractualHours,
                    OvertimeHours = model.OvertimeHours,
                    ContractualEarnings = _payService.ContractualEarnings(model.ContractualHours, model.HourWorked, model.HourlyRate),
                    OvertimeEarnings = 1,
                    NiC = 1,
                    Tax = 1,
                    UnionFee = 1,
                    TotalEarnings = 1,
                    EarningDeduction = 1,
                    NetPayment =1,
                };
                await _payService.CreateAsync(pay);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
