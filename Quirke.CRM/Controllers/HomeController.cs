using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.DataContext;
using Quirke.CRM.Models;
using Quirke.CRM.Services;
using System.Diagnostics;

namespace Quirke.CRM.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        #region PRoperties
        private readonly ICommonService _commonService;
        private readonly ICustomerService _customerService;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context,
           RoleManager<IdentityRole> roleManager, ICommonService commonService, ICustomerService customerService) : base(userManager, null, _context, roleManager)
        {
            _commonService = commonService;
            _customerService = customerService;
        }

        #endregion

        #region Actions

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

        public IActionResult GetCustomerPartial()
        {
            return PartialView("~/Views/Home/_ClientPartial.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            var results = await _commonService.SearchCustomerAsync(term);
            return Json(results);
        }

        public async Task<IActionResult> Compliance(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            var model = new CustomerModel
            {
                Id = customer.Id,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname,
                BirtDate = customer.BirtDate,
                Gender = customer.Gender,
                Mobile = customer.Mobile,
                Email = customer.Email,
                CreatedOn = customer.CreatedOn
            };
            return View(model);
        }

        public async Task<IActionResult> Records(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            var model = new CustomerModel
            {
                Id = customer.Id,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname,
                BirtDate = customer.BirtDate,
                Gender = customer.Gender,
                Mobile = customer.Mobile,
                Email = customer.Email,
                CreatedOn = customer.CreatedOn
            };
            return View(model);
        }
        #endregion

    }
}