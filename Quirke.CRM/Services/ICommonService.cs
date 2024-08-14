namespace Quirke.CRM.Services
{
    public interface ICommonService
    {
        Task<int> GetTotalCustomersAsync();
        Task<int> GetTotalEmployeesAsync();
        Task<int> GetPendingLeaveRequestsAsync();
        Task<int> GetTotalProductsAsync();
        Task<int> GetLowStockProductsCountAsync(int threshold);
    }
}
