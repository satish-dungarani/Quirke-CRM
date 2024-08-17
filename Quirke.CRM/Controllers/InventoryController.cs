using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.Common;
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
            try
            {
                var products = await _inventoryService.GetAllProductsAsync();
                return Json(new { data = products });
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(InventoryController)} - {nameof(GetAllProductData)} - ERROR - {ex}");
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ManageProduct(int? id)
        {

            try
            {
                var model = id.HasValue
                ? await _inventoryService.GetProductByIdAsync(id.Value)
                : new ProductModel();

                model.SupplierList = await _inventoryService.GetSupplierListAsync();
                model.BrandList = await _inventoryService.GetBrandListAsync();

                return PartialView("_ManageProductPartial", model);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(InventoryController)} - {nameof(ManageProduct)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManageProduct(ProductModel model)
        {
            try
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
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(InventoryController)} - {nameof(ManageProduct)} - ERROR - {ex}");
                return View("CustomError");
            }
        }

        [HttpGet]
        public async Task<JsonResult> DeleteProduct(int id)
        {
            try
            {
                var success = await _inventoryService.DeleteProductAsync(id);
                return Json(new { result = success, msg = success ? "Product deleted successfully." : "Failed to delete product." });
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(InventoryController)} - {nameof(DeleteProduct)} - ERROR - {ex}");
                return Json(false);
            }
        }

        #endregion

        #region CurrentStock
        public IActionResult CurrentStock()
        {
            return View();
        }

        public async Task<IActionResult> GetCurrentStockData()
        {
            try
            {
                var list = await _inventoryService.GetLatestInventoryAsync();
                return Json(new { data = list });

            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(InventoryController)} - {nameof(GetCurrentStockData)} - ERROR - {ex}");
                return View("CustomError");
            }
        }
        #endregion

        #region History
        public IActionResult History()
        {
            return View();
        }

        public async Task<IActionResult> GetInventoryHistoryData()
        {
            try
            {
                var list = await _inventoryService.GetAllInventoryHistoryAsync();
                return Json(new { data = list });

            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(InventoryController)} - {nameof(GetInventoryHistoryData)} - ERROR - {ex}");
                return View("CustomError");
            }
        }
        #endregion

    }
}
