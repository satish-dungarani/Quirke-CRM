using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.Models;
using Quirke.CRM.Services;
using System.Diagnostics;

namespace Quirke.CRM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICommonService _commonService;


        public HomeController(ILogger<HomeController> logger, ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var dashboardViewModel = new DashboardViewModel
            {
                TotalCustomers = await _commonService.GetTotalCustomersAsync(),
                TotalEmployees = await _commonService.GetTotalEmployeesAsync(),
                PendingLeaveRequests = await _commonService.GetPendingLeaveRequestsAsync(),
                TotalProducts = await _commonService.GetTotalProductsAsync(),
                LowStockProductsCount = await _commonService.GetLowStockProductsCountAsync(5) // 5 is an example threshold
            };

            return View(dashboardViewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
         
       
    }
}