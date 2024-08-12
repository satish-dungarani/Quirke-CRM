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

        #region Customer

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
                    customerModel.Id = customer.Id;
                }
                else
                {
                    await _customerService.UpdateCustomerAsync(customer);
                }
                TempData["SuccessMessage"] = "Customer details saved successfully!";
            }

            return View(customerModel);
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteCustomerAsync(id);
            return Json(new { result = true, msg = "Customer deleted successfully." });
        }
        #endregion

        #region Compliance
        [HttpGet]
        public async Task<IActionResult> GetCustomerCompliances(int customerId)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);

            var compliances = await _customerService.GetCustomerComplianceByCustomerIdAsync(customerId);
            var data = compliances.Select(c => new
            {
                Id = c.Id,
                CustomerId = customer.Id,
                Firstname = customer.Firstname,
                Lastname = customer.Lastname,
                ComplianceStatus = c.Status,
                TestDate = c.TestDate?.ToString("dd/MM/yyyy"),
                ObservedBy = c.ObservedBy,
                CanTakeService = c.CanTakeService ? "Yes" : "No",
                IsAllergyTestDone = c.IsAllergyTestDone ? "Yes" : "No",
                IsValid = c.TestDate == null || c.TestDate > DateTime.Now.AddMonths(-6)
            }).ToList();

            return Json(new { data });
        }

        [HttpGet]
        public async Task<IActionResult> ManageCompliance(int customerId, int? id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return NotFound();

            var model = new CustomerComplianceModel();
            {
                model.CustomerId = customer.Id;
                model.Firstname = customer.Firstname;
                model.Lastname = customer.Lastname;
                model.BirthDate = customer.BirtDate;
                model.Mobile = customer.Mobile;
            }
            if (id == null)
            {
                var hasActive = await _customerService.HasActiveComplianceAsync(customerId);
                if (hasActive)
                {
                    var customerModel = new CustomerModel
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
                    TempData["WarningMessage"] = "Customer already have active compliance!";

                    return View("~/Views/Customer/Manage.cshtml", customerModel);
                }
            }
            else
            {
                var compliance = await _customerService.GetCustomerComplianceByIdAsync(id.Value);
                if (compliance == null)
                {
                    return NotFound();
                }

                model.Status = compliance.Status;
                model.IsAllergicToColour = compliance.IsAllergicToColour;
                model.AllergicColourDetails = compliance.AllergicColourDetails;
                model.IsDamagedScalp = compliance.IsDamagedScalp;
                model.ScalpDetails = compliance.ScalpDetails;
                model.HasTattoo = compliance.HasTattoo;
                model.TattooDetails = compliance.TattooDetails;
                model.IsAllergicToProduct = compliance.IsAllergicToProduct;
                model.AllergicProductDetails = compliance.AllergicProductDetails;
                model.CanTakeService = compliance.CanTakeService;
                model.IsAllergyTestDone = compliance.IsAllergyTestDone;
                model.TestScheduleOn = compliance.TestScheduleOn;
                model.TestDate = compliance.TestDate;
                model.ObservedBy = compliance.ObservedBy;
                model.CreatedOn = compliance.CreatedOn;
                model.UpdatedOn = compliance.UpdatedOn;
                model.SignatureData = compliance.SignatureData;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageCompliance(CustomerComplianceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Id == 0)
            {
                model.Id = await _customerService.CreateCustomerComplianceAsync(model);
                TempData["SuccessMessage"] = "Compliance created successfully!";
            }
            else
            {
                await _customerService.UpdateCustomerComplianceAsync(model);
                TempData["SuccessMessage"] = "Compliance updated successfully!";
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> ViewCompliance(int customerId, int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(customerId);
            if (customer == null)
                return NotFound();

            var compliance = await _customerService.GetCustomerComplianceByIdAsync(id);
            if (compliance == null)
                return NotFound();


            var model = new CustomerComplianceModel();
            {
                model.CustomerId = customer.Id;
                model.Firstname = customer.Firstname;
                model.Lastname = customer.Lastname;
                model.BirthDate = customer.BirtDate;
                model.Mobile = customer.Mobile;
                model.Status = compliance.Status;
                model.IsAllergicToColour = compliance.IsAllergicToColour;
                model.AllergicColourDetails = compliance.AllergicColourDetails;
                model.IsDamagedScalp = compliance.IsDamagedScalp;
                model.ScalpDetails = compliance.ScalpDetails;
                model.HasTattoo = compliance.HasTattoo;
                model.TattooDetails = compliance.TattooDetails;
                model.IsAllergicToProduct = compliance.IsAllergicToProduct;
                model.AllergicProductDetails = compliance.AllergicProductDetails;
                model.CanTakeService = compliance.CanTakeService;
                model.IsAllergyTestDone = compliance.IsAllergyTestDone;
                model.TestScheduleOn = compliance.TestScheduleOn;
                model.TestDate = compliance.TestDate;
                model.ObservedBy = compliance.ObservedBy;
                model.CreatedOn = compliance.CreatedOn;
                model.UpdatedOn = compliance.UpdatedOn;
                model.IsValid = compliance.TestDate == null || compliance.TestDate > DateTime.Now.AddMonths(-6);
                model.SignatureData = compliance.SignatureData;
            }
            return View(model);
        }
       
        public async Task<IActionResult> DeleteCompliance(int customerId, int id)
        {
            await _customerService.DeleteCustomerComplianceAsync(id);

            var customer = await _customerService.GetCustomerByIdAsync(customerId);
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
            TempData["SuccessMessage"] = "Customer compliance deleted successfully!";

            return View("~/Views/Customer/Manage.cshtml", model);
        }
        #endregion

        #region Record

        #endregion
    }
}
