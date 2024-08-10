using Quirke.CRM.Domain;
using Quirke.CRM.Models;

namespace Quirke.CRM.Services
{
    public interface ICustomerService
    {
        // Customer methods
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int customerId);

        // CustomerCompliance methods
        Task<IEnumerable<CustomerCompliance>> GetAllCustomerCompliancesAsync();
        Task<CustomerCompliance> GetCustomerComplianceByIdAsync(int customerComplianceId);
        Task<int> CreateCustomerComplianceAsync(CustomerComplianceModel model);
        Task UpdateCustomerComplianceAsync(CustomerComplianceModel customerCompliance);
        Task DeleteCustomerComplianceAsync(int customerComplianceId);
        Task<IEnumerable<CustomerCompliance>> GetCustomerComplianceByCustomerIdAsync(int customerId);
        Task<bool> IsAnyCustomerRegisteredByMobileNumberWithActiveComplianceAsync(string mobileNumber);
        Task<bool> HasActiveComplianceAsync(int customerId);
        Task<CustomerComplianceModel?> GetCustomerComplianceByMobile(string mobile);
    }
}
