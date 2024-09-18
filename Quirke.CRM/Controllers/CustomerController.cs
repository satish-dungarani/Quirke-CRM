using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.Common;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain;
using Quirke.CRM.Models;
using Quirke.CRM.Services;
using Rotativa.AspNetCore;

namespace Quirke.CRM.Controllers
{
    [Authorize]
    public class CustomerController : BaseController
    {
        #region Properties
        protected readonly ICustomerService _customerService;
        private static readonly ILog logger = LogHelper.logger;

        public CustomerController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context,
            RoleManager<IdentityRole> roleManager, ICustomerService customerService) : base(userManager, null, _context, roleManager)
        {
            _customerService = customerService;
        }
        #endregion

        #region Customer

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var customers = (await _customerService.GetAllCustomersAsync()).Select(c => new
                {
                    Id = c.Id,
                    Firstname = c.Firstname,
                    Lastname = c.Lastname,
                    BirtDate = c.BirtDate,
                    DispBirtDate = c.BirtDate.ToString("dd MMM yyyy"),
                    Gender = c.Gender,
                    Mobile = c.Mobile,
                    Email = c.Email,
                    DispCreatedOn = c.CreatedOn.ToString("dd MMM yyyy")
                })
                .ToList();
                return Json(new { data = customers });
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(GetCustomers)} - ERROR - {ex}");
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int? id)
        {
            try
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
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(Manage)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(CustomerModel customerModel)
        {
            try
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
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(Manage)} [POST] - ERROR - {ex}");
                return View("CustomError");
            }
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
                return Json(new { result = true, msg = "Customer deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(DeleteCustomer)} - ERROR - {ex}");
                return Json(new { result = false, msg = "Failed to delete customer." });
            }
        }
        #endregion

        #region Compliance
        [HttpGet]
        public async Task<IActionResult> GetCustomerCompliances(int customerId)
        {
            try
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
                    DispTestDate = c.TestDate?.ToString("dd MMM yyyy"),
                    ObservedBy = c.ObservedBy,
                    CanTakeService = c.CanTakeService ? "Yes" : "No",
                    IsAllergyTestDone = c.IsAllergyTestDone ? "Yes" : "No",
                    IsValid = c.TestDate == null || c.TestDate > DateTime.Now.AddMonths(-6)
                }).ToList();

                return Json(new { data });
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(GetCustomerCompliances)} - ERROR - {ex}");
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageCompliance(int customerId, int? id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(customerId);
                if (customer == null)
                    return NotFound();

                var model = new CustomerComplianceModel
                {
                    CustomerId = customer.Id,
                    Firstname = customer.Firstname,
                    Lastname = customer.Lastname,
                    BirthDate = customer.BirtDate,
                    Mobile = customer.Mobile
                };

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
                        return NotFound();

                    model.Status = compliance.Status;
                    model.IsIdentityProvided = compliance.IsIdentityProvided;
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
                    model.SalonSignatureData = compliance.SalonSignatureData;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(ManageCompliance)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageCompliance(CustomerComplianceModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

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

                return RedirectToAction("Manage", new { id = model.CustomerId });
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(ManageCompliance)} [POST] - ERROR - {ex}");
                return View("CustomError");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewCompliance(int customerId, int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(customerId);
                if (customer == null)
                    return NotFound();

                var compliance = await _customerService.GetCustomerComplianceByIdAsync(id);
                if (compliance == null)
                    return NotFound();

                var model = new CustomerComplianceModel
                {
                    CustomerId = customer.Id,
                    Firstname = customer.Firstname,
                    Lastname = customer.Lastname,
                    BirthDate = customer.BirtDate,
                    DispBirthDate = customer.BirtDate.ToString("dd MMM yyyy"),
                    Mobile = customer.Mobile,
                    Status = compliance.Status,
                    IsIdentityProvided = compliance.IsIdentityProvided,
                    IsAllergicToColour = compliance.IsAllergicToColour,
                    AllergicColourDetails = compliance.AllergicColourDetails,
                    IsDamagedScalp = compliance.IsDamagedScalp,
                    ScalpDetails = compliance.ScalpDetails,
                    HasTattoo = compliance.HasTattoo,
                    TattooDetails = compliance.TattooDetails,
                    IsAllergicToProduct = compliance.IsAllergicToProduct,
                    AllergicProductDetails = compliance.AllergicProductDetails,
                    CanTakeService = compliance.CanTakeService,
                    IsAllergyTestDone = compliance.IsAllergyTestDone,
                    TestScheduleOn = compliance.TestScheduleOn,
                    DispTestScheduleOn = compliance.TestScheduleOn?.ToString("dd MMM yyyy"),
                    TestDate = compliance.TestDate,
                    DispTestDate = compliance.TestDate?.ToString("dd MMM yyyy"),
                    ObservedBy = compliance.ObservedBy,
                    CreatedOn = compliance.CreatedOn,
                    DispCreatedOn = compliance.CreatedOn.ToString("dd MMM yyyy"),
                    UpdatedOn = compliance.UpdatedOn,
                    IsValid = compliance.TestDate == null || compliance.TestDate > DateTime.Now.AddMonths(-6),
                    SignatureData = compliance.SignatureData,
                    SalonSignatureData = compliance.SalonSignatureData,
                    TestDueDate = compliance.TestDate?.AddMonths(6).ToString("dd MMM yyyy"),
                };

                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(ViewCompliance)} - ERROR - {ex}");
                return View("Error");
            }
        }

        public async Task<IActionResult> DeleteCompliance(int customerId, int id)
        {
            try
            {
                await _customerService.DeleteCustomerComplianceAsync(id);
                var customer = await _customerService.GetCustomerByIdAsync(customerId);
                var model = new CustomerModel
                {
                    Id = customer.Id,
                    Firstname = customer.Firstname,
                    Lastname = customer.Lastname,
                    BirtDate = customer.BirtDate,
                    DispBirthDate = customer.BirtDate.ToString("dd MMM yyyy"),
                    Gender = customer.Gender,
                    Mobile = customer.Mobile,
                    Email = customer.Email,
                    CreatedOn = customer.CreatedOn
                };
                TempData["SuccessMessage"] = "Customer compliance deleted successfully!";
                return View("~/Views/Customer/Manage.cshtml", model);
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(DeleteCompliance)} - ERROR - {ex}");
                return View("CustomError");
            }
        }



        public async Task<IActionResult> DownloadCustomerCompliancePdf(int id)
        {
            // Fetch customer compliance data
            var compliance = await _customerService.GetCustomerComplianceModelByIdAsync(id);
            var customer = await _customerService.GetCustomerByIdModelAsync(compliance.CustomerId);

            var model = new CustomerViewModel
            {
                Customer = customer,
                Compliance = compliance
            };

            return new ViewAsPdf("CustomerCompliancePdf", model)
            {
                FileName = $"{customer.Firstname}_ComplianceForm.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }
        #endregion

        #region Record
        [HttpGet]
        public async Task<JsonResult> GetAllCustomerRecordData(int CustomerId)
        {
            try
            {
                var records = await _customerService.GetAllCustomerRecordsByCustomerIdAsync(CustomerId);
                return Json(new { data = records });
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(GetAllCustomerRecordData)} - ERROR - {ex}");
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageCustomerRecord(int customerId, int? id)
        {
            try
            {
                var model = id.HasValue
                    ? await _customerService.GetCustomerRecordByIdAsync(id.Value)
                    : new CustomerRecordModel() { CustomerId = customerId, ServiceDate = DateTime.UtcNow };

                model.ProductList = await _customerService.GetProductListAsync();
                model.TreatmentList = await _customerService.GetTreatmentListAsync();
                model.AttendedEmployeeList = await _customerService.GetEmployeeListAsync();

                return PartialView("_ManageCustomerRecordPartial", model);
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(ManageCustomerRecord)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManageCustomerRecord(CustomerRecordModel model)
        {
            try
            {
                ModelState.Remove(nameof(model.Id));
                if (!ModelState.IsValid)
                {
                    model.ProductList = await _customerService.GetProductListAsync();
                    model.TreatmentList = await _customerService.GetTreatmentListAsync();
                    model.AttendedEmployeeList = await _customerService.GetEmployeeListAsync();
                    return PartialView("_ManageCustomerRecordPartial", model);
                }

                string strMessage;
                if (model.Id == 0)
                {
                    strMessage = "Customer record saved successfully";
                    await _customerService.CreateCustomerRecordAsync(model);
                }
                else
                {
                    strMessage = "Customer record updated successfully";
                    await _customerService.UpdateCustomerRecordAsync(model);
                }

                return Json(new { result = true, msg = strMessage });
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(ManageCustomerRecord)} [POST] - ERROR - {ex}");
                return Json(new { result = false, msg = "Failed to save customer record." });
            }
        }

        [HttpGet]
        public async Task<JsonResult> DeleteCustomerRecord(int id)
        {
            try
            {
                var success = await _customerService.DeleteCustomerRecordAsync(id);
                return Json(new { result = success, msg = success ? "Customer record deleted successfully." : "Failed to delete customer record." });
            }
            catch (Exception ex)
            {
                logger.Error($"{nameof(CustomerController)} - {nameof(DeleteCustomerRecord)} - ERROR - {ex}");
                return Json(new { result = false, msg = "Failed to delete customer record." });
            }
        }


        public async Task<IActionResult> DownloadCustomerRecordPdf(int customerId)
        {
            // Fetch customer data
            var customer = await _customerService.GetCustomerByIdModelAsync(customerId);
            var records = await _customerService.GetAllCustomerRecordsByCustomerIdAsync(customerId);

            var model = new CustomerViewModel
            {
                Customer = customer,
                CustomerRecords = records
            };

            return new ViewAsPdf("CustomerRecordPdf", model)
            {
                FileName = $"{customer.Firstname}_CustomerRecord.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }
        #endregion
    }
}
