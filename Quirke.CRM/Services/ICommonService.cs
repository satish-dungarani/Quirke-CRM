using Microsoft.AspNetCore.Mvc.Rendering;

namespace Quirke.CRM.Services
{
    public interface ICommonService
    {
        Task<int> GetTotalCustomersAsync();
        Task<int> GetTotalEmployeesAsync();
        Task<int> GetPendingLeaveRequestsAsync();
        Task<int> GetTotalProductsAsync();
        Task<int> GetLowStockProductsCountAsync(int threshold);
        Task<IEnumerable<SelectListItem>> GetCustomerListAsync();
        Task<IEnumerable<object>> SearchCustomerAsync(string term);
    }
}
