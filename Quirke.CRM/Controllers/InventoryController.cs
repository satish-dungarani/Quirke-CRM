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
    public class InventoryController : BaseController
    {
        protected readonly IInventoryService _inventoryService;
        public InventoryController(UserManager<ApplicationUser> userManager, ApplicationDbContext _context,
            RoleManager<IdentityRole> roleManager, IInventoryService inventoryService) : base(userManager, null, _context, roleManager)
        {
            _inventoryService = inventoryService;
        }

        #region Poducts
        public IActionResult Products()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAllProductData()
        {
            var products = await _inventoryService.GetAllProductsAsync();
            return Json(new { data = products });
        }

        [HttpGet]
        public async Task<IActionResult> ManageProduct(int? id)
        {
            var model = id.HasValue
                ? await _inventoryService.GetProductByIdAsync(id.Value)
                : new ProductModel();

            model.SupplierList = await _inventoryService.GetSupplierListAsync();
            model.BrandList = await _inventoryService.GetBrandListAsync();

            return PartialView("_ManageProductPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageProduct(ProductModel model)
        {
            ModelState.Remove(nameof(model.Id));
            if (!ModelState.IsValid)
            {
                model.SupplierList = await _inventoryService.GetSupplierListAsync();
                model.BrandList = await _inventoryService.GetBrandListAsync();
                return PartialView("_ManageProductPartial", model);
            }
            string strMessage;
            if (model.Id == 0)
            {
                strMessage = "Product saved successfully";
                await _inventoryService.CreateProductAsync(model);
            }
            else
            {
                strMessage = "Product updated successfully";
                await _inventoryService.UpdateProductAsync(model);
            }

            return Json(new { result = true, msg = strMessage });
        }

        [HttpGet]
        public async Task<JsonResult> DeleteProduct(int id)
        {
            var success = await _inventoryService.DeleteProductAsync(id);
            return Json(new { result = success, msg = success ? "Product deleted successfully." : "Failed to delete product." });
        }

        #endregion

        #region CurrentStock
        public IActionResult CurrentStock()
        {
            return View();
        }

        public async Task<IActionResult> GetCurrentStockData()
        {
            var list = await _inventoryService.GetLatestInventoryAsync();
            return Json(new { data = list });
        }
        #endregion

        #region History
        public IActionResult History()
        {
            return View();
        }

        public async Task<IActionResult> GetInventoryHistoryData()
        {
            var list = await _inventoryService.GetAllInventoryHistoryAsync();
            return Json(new { data = list });
        }
        #endregion

    }
}
