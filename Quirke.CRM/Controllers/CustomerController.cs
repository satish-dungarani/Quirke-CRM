using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain;
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

        [HttpGet]
        public async Task<IActionResult> Manage(int? id)
        {
            CustomerModel customerModel;

            if (id == null)
            {
                customerModel = new CustomerModel();
            }
            else
            {
                var customer = await _customerService.GetCustomerByIdAsync(id.Value);
                customerModel = new CustomerModel
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
            }

            return View(customerModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(CustomerModel customerModel)
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Id = customerModel.Id,
                    Firstname = customerModel.Firstname,
                    Lastname = customerModel.Lastname,
                    BirtDate = customerModel.BirtDate ?? DateTime.Now,
                    Gender = customerModel.Gender,
                    Mobile = customerModel.Mobile,
                    Email = customerModel.Email,
                    CreatedOn = DateTime.UtcNow
                };

                if (customer.Id == 0)
                {
                    await _customerService.CreateCustomerAsync(customer);
                }
                else
                {
                    await _customerService.UpdateCustomerAsync(customer);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(customerModel);
        }
    }
}
