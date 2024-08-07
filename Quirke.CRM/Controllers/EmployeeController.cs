using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.DataContext;
using Quirke.CRM.Models;
using Quirke.CRM.Services;

namespace Quirke.CRM.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        protected readonly IEmployeeService _employeeService;
        public EmployeeController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context, RoleManager<IdentityRole> roleManager,
            IEmployeeService employeeService) : base(userManager, null, _context, roleManager)
        {
            _employeeService = employeeService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
