using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Logging;
using Quirke.CRM.Common;
using Quirke.CRM.DataContext;
using Quirke.CRM.Models;
using Quirke.CRM.Services;
using System.Data.Common;

namespace Quirke.CRM.Controllers
{
    [Authorize]
    public class EmployeeController : BaseController
    {
        protected readonly IEmployeeService _employeeService;
        protected readonly IMasterService _masterService;
        public EmployeeController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context, RoleManager<IdentityRole> roleManager,
            IEmployeeService employeeService, IMasterService masterService) : base(userManager, null, _context, roleManager)
        {
            _employeeService = employeeService;
            _masterService = masterService;
        }

        #region Utility
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
                var result = await _employeeService.AddOrUpdateEmployeeLeaveAsync(model);
                if (result)
                {
                    TempData["SuccessMessage"] = "Employee leave saved successfully.";
                    return RedirectToAction(nameof(Manage), new { id = model.EmployeeId });
                }
                else
                {
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

    }
}
