using Microsoft.AspNetCore.Mvc.Rendering;
using Quirke.CRM.Domain;
using Quirke.CRM.Models;

namespace Quirke.CRM.Services
{
    public interface ICustomerService
    {
        
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int customerId);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int customerId);

        Task<IEnumerable<CustomerCompliance>> GetAllCustomerCompliancesAsync();
        Task<CustomerCompliance> GetCustomerComplianceByIdAsync(int customerComplianceId);
        Task<int> CreateCustomerComplianceAsync(CustomerComplianceModel model);
        Task UpdateCustomerComplianceAsync(CustomerComplianceModel customerCompliance);
        Task DeleteCustomerComplianceAsync(int customerComplianceId);
        Task<IEnumerable<CustomerCompliance>> GetCustomerComplianceByCustomerIdAsync(int customerId);
        Task<bool> IsAnyCustomerRegisteredByMobileNumberWithActiveComplianceAsync(string mobileNumber);
        Task<bool> HasActiveComplianceAsync(int customerId);
        Task<CustomerComplianceModel?> GetCustomerComplianceByMobile(string mobile);

        Task<IEnumerable<CustomerRecordModel>> GetAllCustomerRecordsByCustomerIdAsync(int customerId);
        Task<CustomerRecordModel> GetCustomerRecordByIdAsync(int id);
        Task CreateCustomerRecordAsync(CustomerRecordModel customerRecordModel);
        Task UpdateCustomerRecordAsync(CustomerRecordModel customerRecordModel);
        Task<bool> DeleteCustomerRecordAsync(int id);
        Task<IEnumerable<SelectListItem>> GetProductListAsync();
        Task<IEnumerable<SelectListItem>> GetTreatmentListAsync();
        Task<IEnumerable<SelectListItem>> GetEmployeeListAsync();

        Task<CustomerModel> GetCustomerByIdModelAsync(int customerId);
        Task<CustomerComplianceModel> GetCustomerComplianceModelByIdAsync(int id);
    }
}
