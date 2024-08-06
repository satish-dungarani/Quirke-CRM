using Quirke.CRM.Domain;

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
        Task<CustomerCompliance> CreateCustomerComplianceAsync(CustomerCompliance customerCompliance);
        Task UpdateCustomerComplianceAsync(CustomerCompliance customerCompliance);
        Task DeleteCustomerComplianceAsync(int customerComplianceId);
        Task<IEnumerable<CustomerCompliance>> GetCustomerComplianceByCustomerIdAsync(int customerId);
        Task<bool> IsAnyCustomerRegisteredByMobileNumberWithActiveComplianceAsync(string mobileNumber);
    }
}
