using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.DataContext;
using Quirke.CRM.Models;
using Quirke.CRM.Services;

namespace Quirke.CRM.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        protected readonly ICustomerService _customerService;
        public CustomerController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context, RoleManager<IdentityRole> roleManager, ICustomerService customerService) : base(userManager, null, _context, roleManager)
        {
            _customerService = customerService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = (await _customerService.GetAllCustomersAsync()).Select(c => new
            {
                Id = c.Id,
                Firstname = c.Firstname,
                Lastname = c.Lastname,
                BirtDate = c.BirtDate.ToString("yyyy-MM-dd"), // Format as needed
                Gender = c.Gender,
                Mobile = c.Mobile,
                Email = c.Email,
                CreatedOn = c.CreatedOn.ToString("yyyy-MM-dd HH:mm:ss") // Format as needed
            })
            .ToList();

            return Json(new { data = customers });
        }
    }
}
