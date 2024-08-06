using Microsoft.EntityFrameworkCore;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain;

namespace Quirke.CRM.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Customer methods
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        // CustomerCompliance methods
        public async Task<IEnumerable<CustomerCompliance>> GetAllCustomerCompliancesAsync()
        {
            return await _context.CustomerCompliances.ToListAsync();
        }

        public async Task<CustomerCompliance> GetCustomerComplianceByIdAsync(int customerComplianceId)
        {
            return await _context.CustomerCompliances.FindAsync(customerComplianceId);
        }

        public async Task<CustomerCompliance> CreateCustomerComplianceAsync(CustomerCompliance customerCompliance)
        {
            _context.CustomerCompliances.Add(customerCompliance);
            await _context.SaveChangesAsync();
            return customerCompliance;
        }

        public async Task UpdateCustomerComplianceAsync(CustomerCompliance customerCompliance)
        {
            _context.Entry(customerCompliance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerComplianceAsync(int customerComplianceId)
        {
            var customerCompliance = await _context.CustomerCompliances.FindAsync(customerComplianceId);
            if (customerCompliance != null)
            {
                _context.CustomerCompliances.Remove(customerCompliance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CustomerCompliance>> GetCustomerComplianceByCustomerIdAsync(int customerId)
        {
            return await _context.CustomerCompliances
                                 .Where(cc => cc.CustomerId == customerId)
                                 .ToListAsync();
        }

        public async Task<bool> IsAnyCustomerRegisteredByMobileNumberWithActiveComplianceAsync(string mobileNumber)
        {
            var customer = await _context.Customers
                                         .FirstOrDefaultAsync(c => c.Mobile == mobileNumber);
            if (customer == null)
            {
                return false;
            }

            return await _context.CustomerCompliances
                                 .AnyAsync(cc => cc.CustomerId == customer.Id && cc.Status.ToLower() == "active"); 
        }
    }
}
