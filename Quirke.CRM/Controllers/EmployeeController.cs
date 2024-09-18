using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using Quirke.CRM.Common;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain;
using Quirke.CRM.Models;
using Quirke.CRM.Services;

namespace Quirke.CRM.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        #region MyRegion

        protected readonly IEmployeeService _employeeService;
        protected readonly IMasterService _masterService;
        protected readonly ILeaveRequestService _leaveRequestService;
        public EmployeeController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context, RoleManager<IdentityRole> roleManager,
            IEmployeeService employeeService, IMasterService masterService, ILeaveRequestService leaveRequestService) : base(userManager, null, _context, roleManager)
        {
            _employeeService = employeeService;
            _masterService = masterService;
            _leaveRequestService = leaveRequestService;
        }

        #endregion

        #region Utility
        private async Task<List<SelectListItem>> GetEmployeeList()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();

                var selectListItems = employees
                    .Select(item => new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Firstname + " " + item.Lastname
                    }).ToList();
                return selectListItems;
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(GetEmployeeList)} - ERROR - {ex}");
                return new List<SelectListItem>();
            }

        }
        private async Task<List<SelectListItem>> GetLeaveType(int masterTypeId)
        {
            try
            {
                var leavetypes = await _masterService.GetAllByMasterTypeIdAsync(masterTypeId);

                var selectListItems = leavetypes
                    .Select(item => new SelectListItem
                    {
                        Value = item.Id.ToString(),
                        Text = item.Name
                    }).ToList();
                return selectListItems;
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(GetLeaveType)} - ERROR - {ex}");
                return new List<SelectListItem>();
            }
        }
        #endregion

        #region Employee


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                var data = new
                {
                    data = employees
                };
                return Json(data);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(GetEmployees)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int? id)
        {
            try
            {
                if (id == null)
                {
                    return View(new EmployeeModel());
                }

                var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(Manage)} - ERROR - {ex}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(EmployeeModel model, IFormFile pictureFile, IFormFile identityDocumentFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (pictureFile != null && pictureFile.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await pictureFile.CopyToAsync(stream);
                            model.Picture = Convert.ToBase64String(stream.ToArray());
                        }
                    }

                    if (identityDocumentFile != null && identityDocumentFile.Length > 0)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await identityDocumentFile.CopyToAsync(stream);
                            model.IdentityDocument = Convert.ToBase64String(stream.ToArray());
                        }
                    }

                    if (model.Id == 0)
                    {
                        await _employeeService.AddEmployeeAsync(model);
                    }
                    else
                    {
                        await _employeeService.UpdateEmployeeAsync(model);
                    }

                    return RedirectToAction("EmployeeProfile");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(Manage)} - ERROR - {ex}");
                return View("CustomError");
            }

        }

        public IActionResult DownloadDocument(int id)
        {
            try
            {

                var employee = _employeeService.GetEmployeeByIdAsync(id).Result;

                if (employee == null || string.IsNullOrEmpty(employee.IdentityDocument))
                {
                    return NotFound();
                }

                var documentBytes = Convert.FromBase64String(employee.IdentityDocument);
                var fileName = $"IdentityDocument_{employee.Firstname}_{employee.Lastname}.pdf";

                return File(documentBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(DownloadDocument)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        #endregion

        #region Employee Leave

        public async Task<IActionResult> GetEmployeeLeaves(int employeeId)
        {
            try
            {
                var leaves = await _employeeService.GetEmployeeLeavesByEmployeeIdAsync(employeeId);
                return Json(new { data = leaves });
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(GetEmployeeList)} - ERROR - {ex}");
                return Json(null);
            }
        }

        public async Task<IActionResult> ManageLeave(int employeeId, int id = 0)
        {
            try
            {
                EmployeeLeaveModel model = id == 0
                    ? new EmployeeLeaveModel { EmployeeId = employeeId }
                    : await _employeeService.GetEmployeeLeaveByIdAsync(id);

                if (model == null)
                {
                    return NotFound();
                }
                model.LeaveTypeList = await GetLeaveType((int)MasterType.LeaveType);
                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(ManageLeave)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageLeave(EmployeeLeaveModel model)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    bool isDuplicate = await _employeeService.IsDuplicateLeaveAsync(model.EmployeeId, model.LeaveTypeId, model.Id > 0 ? (int?)model.Id : null);

                    if (isDuplicate)
                    {
                        model.LeaveTypeList = await GetLeaveType((int)MasterType.LeaveType);
                        TempData["ErrorMessage"] = "A leave record already exists for this leave type and employee.";
                        return View(model);
                    }

                    var result = await _employeeService.AddOrUpdateEmployeeLeaveAsync(model);
                    if (result)
                    {
                        TempData["SuccessMessage"] = "Employee leave saved successfully.";
                        return RedirectToAction(nameof(Manage), new { id = model.EmployeeId });
                    }
                    else
                    {
                        model.LeaveTypeList = await GetLeaveType((int)MasterType.LeaveType);
                        TempData["WarningMessage"] = "Failed to save employee leave.";
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(ManageLeave)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        public async Task<IActionResult> DeleteLeave(int employeeId, int id)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeLeaveAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Employee leave deleted successfully.";
                }
                else
                {
                    TempData["WarningMessage"] = "Failed to delete employee leave.";
                }

                return RedirectToAction(nameof(Manage), new { id = employeeId });
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(DeleteLeave)} - ERROR - {ex}");
                return View("CustomError");
            }
            
        }

        #endregion

        #region Leave Request

        public IActionResult LeaveRequest() { return View(); }

        public async Task<IActionResult> GetLeaveRequests()
        {
            try
            {
                var requests = await _leaveRequestService.GetAllLeaveRequestsAsync();
                return Json(new { data = requests });
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(GetLeaveRequests)} - ERROR - {ex}");
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageRequest(int? id)
        {
            try
            {
                var model = new LeaveRequestModel();

                if (id != null && id > 0)
                {
                    model = await _leaveRequestService.GetLeaveRequestByIdAsync(id.Value);
                }
                model.LeaveTypeList = await GetLeaveType((int)MasterType.LeaveType);
                model.EmployeeList = await GetEmployeeList();
                return PartialView("~/Views/Employee/_ManageRequestPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(ManageRequest)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManageRequests(LeaveRequestModel model, IFormFile frmLeaveFile)
        {
            try
            {
                ModelState.Remove(nameof(model.Id));

                if (!ModelState.IsValid) { return Json(null); };

                if (model.LeaveDuration == "Half Day")
                {
                    model.AppliedDays = 0.5m;
                }
                else if (model.LeaveDuration == "Full Day")
                {
                    model.AppliedDays = 1.0m;
                }
                else
                {
                    TimeSpan difference = Convert.ToDateTime(model.EndDate) - Convert.ToDateTime(model.StartDate);
                    model.AppliedDays = difference.Days + 1;
                }

                decimal remainingDays = await _employeeService.RmainingLeaves(model.EmployeeId, model.LeaveTypeId);

                if (model.AppliedDays > remainingDays)
                {
                    return Json(new
                    {
                        result = false,
                        msg = "Remainig leaves - " + remainingDays
                    });
                }

                if (frmLeaveFile != null && frmLeaveFile.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await frmLeaveFile.CopyToAsync(stream);
                        model.Document = Convert.ToBase64String(stream.ToArray());
                    }
                }
                string strMessage;
                if (model.Id == 0)
                {
                    await _leaveRequestService.CreateLeaveRequestAsync(model);
                    strMessage = "Leave request saved successfully!";
                }
                else
                {
                    await _leaveRequestService.UpdateLeaveRequestAsync(model);
                    strMessage = "Leave request updated successfully!";
                }

                var empLeave = await _employeeService.GetEmployeeLeavesByEmployeeIdAsync(model.EmployeeId);

                return Json(new { result = true, msg = strMessage });
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(ManageRequests)} - ERROR - {ex}");
                return Json(new { result = false, msg = ex.Message });
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> ViewRequest(int id)
        {
            try
            {
                var model = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
                return PartialView("~/Views/Employee/_ViewRequestPartial.cshtml", model);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(ViewRequest)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        public async Task<IActionResult> DeleteRequest(int id)
        {
            try
            {
                var model = await _leaveRequestService.DeleteLeaveRequestAsync(id);
                return Json(new { result = true, msg = "Leave request successfully deleted." });
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(DeleteRequest)} - ERROR - {ex}");
                return Json(new { result = false, msg = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetLeaveTypesByEmployee(int employeeId)
        {
            try
            {
                var leaveTypes = await _employeeService.GetLeaveTypesForEmployeeAsync(employeeId);

                return Json(leaveTypes);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(GetLeaveTypesByEmployee)} - ERROR - {ex}");
                return Json(null);
            }
        }

        public async Task<IActionResult> DownloadLeaveRequestDocument(int id)
        {
            try
            {
                var lr = await _leaveRequestService.GetLeaveRequestByIdAsync(id);

                if (lr == null || string.IsNullOrEmpty(lr.Document))
                {
                    return NotFound();
                }

                var documentBytes = Convert.FromBase64String(lr.Document);
                var fileName = $"Leave_Request_Document.pdf";

                return File(documentBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(DownloadLeaveRequestDocument)} - ERROR - {ex}");
                return View("CustomError");
            }
            
        }
        #endregion

        public async Task<IActionResult> EmployeeProfile(int page = 1, int pageSize = 6)
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesPagingAsync(page, pageSize);

                var totalItems = _context.Employees.Count();

                var pagedResult = new PagedResult<EmployeeModel>
                {
                    Items = employees,
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalItems = totalItems
                };
                return View(pagedResult);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(EmployeeController)} - {nameof(EmployeeProfile)} - ERROR - {ex}");
                return View("CustomError");
            }
        }
    }
}
