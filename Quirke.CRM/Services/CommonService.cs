using Microsoft.EntityFrameworkCore;
using Quirke.CRM.DataContext;

namespace Quirke.CRM.Services
{
    public class CommonService : ICommonService
    {
        private readonly ApplicationDbContext _context;

        public CommonService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalCustomersAsync()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<int> GetTotalEmployeesAsync()
        {
            return await _context.Employees.CountAsync();
        }

        public async Task<int> GetPendingLeaveRequestsAsync()
        {
            return await _context.LeaveRequests.CountAsync(lr => lr.RequestStatus == "Pending");
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetLowStockProductsCountAsync(int threshold)
        {
            return await _context.Products.CountAsync(p => p.QuantityInStock <= threshold);
        }
    }
}
