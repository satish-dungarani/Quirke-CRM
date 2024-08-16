using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.Common;
using Quirke.CRM.DataContext;
using Quirke.CRM.Models;
using Quirke.CRM.Services;
using System.Diagnostics;

namespace Quirke.CRM.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ICommonService _commonService;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context,
           RoleManager<IdentityRole> roleManager, ICommonService commonService) : base(userManager, null, _context, roleManager)
        {
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