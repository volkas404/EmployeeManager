using Microsoft.AspNetCore.Mvc;
using Pay1193.Models;
using Pay1193.Services;
using Pay1193.Services.Implement;

namespace Pay1193.Controllers
{
    public class PayController : Controller
    {
        private readonly IPayService _payService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PayController(IPayService payService, IWebHostEnvironment webHostEnvironment)
        {
            _payService = payService;
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
    }
}
