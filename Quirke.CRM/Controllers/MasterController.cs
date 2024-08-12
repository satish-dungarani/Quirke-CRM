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
        public MasterController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context, RoleManager<IdentityRole> roleManager, IMasterService masterService) : base(userManager, null, _context, roleManager)
        {
            _masterService = masterService;
        }


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
    }
}
