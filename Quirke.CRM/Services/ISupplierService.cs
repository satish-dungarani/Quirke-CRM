using Quirke.CRM.Models;

namespace Quirke.CRM.Services
{
    public interface ISupplierService
    {
        Task<SupplierModel> CreateSupplierAsync(SupplierModel model);

        Task<SupplierModel> GetSupplierByIdAsync(int id);

        Task<IEnumerable<SupplierModel>> GetAllSuppliersAsync();

        Task<SupplierModel> UpdateSupplierAsync(SupplierModel model);

        Task<bool> DeleteSupplierAsync(int id);
    }
}
