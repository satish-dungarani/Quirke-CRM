using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain.Services;
using Quirke.CRM.Models;
using Quirke.CRM.Services;

namespace Quirke.CRM.Controllers
{
    public class MasterController : BaseController
    {
        protected readonly IMasterService _masterService;
        protected readonly ISupplierService _supplierService;
        public MasterController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context, RoleManager<IdentityRole> roleManager, IMasterService masterService, ISupplierService supplierService) : base(userManager, null, _context, roleManager)
        {
            _masterService = masterService;
            _supplierService = supplierService;
        }


        #region Masters

        public IActionResult Index(int id)
        {
            ViewBag.MasterTypeId = id;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetMaster(int typeId)
        {
            var masters = await _masterService.GetAllByMasterTypeIdAsync(typeId);

            var data = new
            {
                data = masters
            };

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> ManageMaster(int typeId, int? id)
        {
            var model = new MasterModel();
            if (id == null)
            {
                model.MasterTypeId = typeId;
            }
            else
            {
                model = await _masterService.GetByIdAsync(id.Value);
            }
            return PartialView("~/Views/Master/_ManageMasterPartial.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageMaster(MasterModel model)
        {
            if (model.Id == 0)
            {
                await _masterService.CreateAsync(model);
            }
            else
            {
                await _masterService.UpdateAsync(model);
            }
            TempData["SuccessMessage"] = "Saved successfully.";
            return RedirectToAction("Index", new { id = model.MasterTypeId });
        }

        public async Task<IActionResult> DeleteMaster(int id)
        {
            var model = await _masterService.DeleteAsync(id);
            return Json(new { result = true, msg = "Deleted successfully." });
        }
        #endregion

        #region Suppliers
        public IActionResult Suppliers()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliersData()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            return Json(new { data = suppliers });
        }

        public async Task<IActionResult> ManageSupplier(int? id)
        {
            SupplierModel supplier;

            if (id.HasValue && id.Value > 0)
            {
                supplier = await _supplierService.GetSupplierByIdAsync(id.Value);
                if (supplier == null)
                {
                    return NotFound();
                }
            }
            else
            {
                supplier = new SupplierModel();
            }

            return PartialView("_ManageSuppliersPartial", supplier);
        }

        [HttpPost]
        public async Task<IActionResult> ManageSupplier(SupplierModel model)
        {
            ModelState.Remove(nameof(model.Id));

            if (!ModelState.IsValid) { return Json(null); };

            string strMessage;
            if (model.Id == 0)
            {
                strMessage = "Supplier data saved successfully";
                await _supplierService.CreateSupplierAsync(model);
            }
            else
            {
                strMessage = "Supplier data updated successfully";
            }   await _supplierService.UpdateSupplierAsync(model);

            return Json(new { result = true, msg = strMessage });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            bool result = await _supplierService.DeleteSupplierAsync(id);

            if (result)
            {
                return Json(new { result = true, msg = "Supplier deleted successfully." });
            }
            else
            {
                return Json(new { result = false, msg = "Error deleting supplier." });
            }
        }

        #endregion
    }
}
