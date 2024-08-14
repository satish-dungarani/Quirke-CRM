using Microsoft.AspNetCore.Mvc.Rendering;
using Quirke.CRM.Models;

namespace Quirke.CRM.Services
{
    public interface IInventoryService
    {
        Task<ProductModel> CreateProductAsync(ProductModel model);
        Task<ProductModel> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();
        Task<ProductModel> UpdateProductAsync(ProductModel model);
        Task<bool> DeleteProductAsync(int id);
        Task<IEnumerable<SelectListItem>> GetSupplierListAsync();
        Task<IEnumerable<SelectListItem>> GetBrandListAsync();
        Task<IEnumerable<InventoryHistoryModel>> GetAllInventoryHistoryByProductsAsync(int productId);
        Task<IEnumerable<InventoryHistoryModel>> GetLatestInventoryAsync();
        Task<IEnumerable<InventoryHistoryModel>> GetAllInventoryHistoryAsync();
    }
}
