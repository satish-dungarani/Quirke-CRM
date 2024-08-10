using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quirke.CRM.Domain;
using Quirke.CRM.Models;
using Quirke.CRM.Services;

namespace Quirke.CRM.Controllers
{
    public class QuirkeController : Controller
    {
        protected readonly ICustomerService _customerService;
        public QuirkeController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Compliance(string mobileNumber)
        {

            //var isActive = await _customerService.IsAnyCustomerRegisteredByMobileNumberWithActiveComplianceAsync(mobileNumber);
            
            var compliance = await _customerService.GetCustomerComplianceByMobile(mobileNumber);
            if (compliance == null)
                return NotFound();

            //if (isActive)
            if (compliance.Id > 0)
            {
                TempData["WarningMessage"] = "You already have active compliance or compliance request!";
                return View("~/Views/Quirke/ViewCompliance.cshtml", compliance);
            }
            else
            {
                compliance.Mobile = mobileNumber;
                return View(compliance);
            }
        }
        [HttpPost]
        public async Task<IActionResult> ComplianceSubmit(CustomerComplianceModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Quirke/Compliance.cshtml", model);
            }

            var isActive = await _customerService.IsAnyCustomerRegisteredByMobileNumberWithActiveComplianceAsync(model.Mobile);

            if (isActive)
            {
                TempData["WarningMessage"] = "You already have active compliance or compliance request!";
                return View("~/Views/Quirke/Index.cshtml");
            }

            if (model.CustomerId == 0)
            {
                var customer = new Customer()
                {
                    BirtDate = model.BirthDate.Value,
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Mobile = model.Mobile,
                    CreatedOn = DateTime.UtcNow,
                    Gender = "Female"
                };

                customer = await _customerService.CreateCustomerAsync(customer);
                model.CustomerId = customer.Id;
            }

            if (model.Id == 0)
            {
                model.Status = "Pending";
                model.CreatedOn = DateTime.UtcNow;

                model.Id = await _customerService.CreateCustomerComplianceAsync(model);
                TempData["SuccessMessage"] = "Compliance created successfully!";
            }
            else
            {
                await _customerService.UpdateCustomerComplianceAsync(model);
                TempData["SuccessMessage"] = "Compliance updated successfully!";
            }
            return View("~/Views/Quirke/ViewCompliance.cshtml", model);
        }


    }
}
