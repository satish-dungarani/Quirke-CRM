using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Quirke.CRM.Common;
using Quirke.CRM.DataContext;
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
            var employees = await _employeeService.GetAllEmployeesAsync();

            var selectListItems = employees
                .Select(item => new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Firstname + " " + item.Lastname
                }).ToList();
            return selectListItems;
        }
        private async Task<List<SelectListItem>> GetLeaveType(int masterTypeId)
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
        #endregion

        #region Employee


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();

            var data = new
            {
                data = employees
            };

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> Manage(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(EmployeeModel model, IFormFile pictureFile, IFormFile identityDocumentFile)
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

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult DownloadDocument(int id)
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

        #endregion

        #region Employee Leave

        public async Task<IActionResult> GetEmployeeLeaves(int employeeId)
        {
            var leaves = await _employeeService.GetEmployeeLeavesByEmployeeIdAsync(employeeId);
            return Json(new { data = leaves });
        }

        public async Task<IActionResult> ManageLeave(int employeeId, int id = 0)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageLeave(EmployeeLeaveModel model)
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

        public async Task<IActionResult> DeleteLeave(int employeeId, int id)
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

        #endregion

        #region Leave Request

        public IActionResult LeaveRequest() { return View(); }

        public async Task<IActionResult> GetLeaveRequests()
        {
            var requests = await _leaveRequestService.GetAllLeaveRequestsAsync();
            return Json(new { data = requests });
        }

        [HttpGet]
        public async Task<IActionResult> ManageRequest(int? id)
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

        [HttpPost]
        public async Task<IActionResult> ManageRequest(LeaveRequestModel model, IFormFile frmLeaveFile)
        {
            decimal remainingDays = await _employeeService.RmainingLeaves(model.EmployeeId, model.LeaveTypeId);
            if (remainingDays > 0)
            {
                return View(model);
            }

            if (frmLeaveFile != null && frmLeaveFile.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await frmLeaveFile.CopyToAsync(stream);
                    model.Document = Convert.ToBase64String(stream.ToArray());
                }
            }
            if (model.Id == 0)
            {
                await _leaveRequestService.CreateLeaveRequestAsync(model);
            }
            else
            {
                await _leaveRequestService.UpdateLeaveRequestAsync(model);
            }
            return RedirectToAction(nameof(LeaveRequest));
        }

        [HttpGet]
        public async Task<IActionResult> ViewRequest(int id)
        {
            var model = await _leaveRequestService.GetLeaveRequestByIdAsync(id);
            return PartialView("~/Views/Employee/_ViewRequestPartial.cshtml", model);
        }

        public async Task<IActionResult> DeleteRequest(int id)
        {
            var model = await _leaveRequestService.DeleteLeaveRequestAsync(id);
            return Json(new { result = true , msg = "Leave request successfully deleted." });
        }

        [HttpGet]
        public async Task<JsonResult> GetLeaveTypesByEmployee(int employeeId)
        {
            var leaveTypes = await _employeeService.GetLeaveTypesForEmployeeAsync(employeeId); 

            return Json(leaveTypes);
        }

        public async Task<IActionResult> DownloadLeaveRequestDocument(int id)
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
        #endregion

    }
}
