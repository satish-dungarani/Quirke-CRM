using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quirke.CRM.DataContext;
using Quirke.CRM.Models;

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

        public async Task<IEnumerable<SelectListItem>> GetCustomerListAsync()
        {
            return await _context.Customers
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = $"{e.Firstname} {e.Lastname}"
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<object>> SearchCustomerAsync(string term)
        {
            var terms = term.Split(' ');

            return await _context.Customers
                .Where(c =>
                    terms.All(t => c.Firstname.Contains(t) || c.Lastname.Contains(t))
                    || c.Mobile.Contains(term)
                )
                .Select(e => new
                {
                    label = e.Firstname + " " + e.Lastname,
                    value = e.Id
                })
                .ToListAsync();
        }
    }
}
