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

    }
}
