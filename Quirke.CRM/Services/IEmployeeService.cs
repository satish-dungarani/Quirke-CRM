using Quirke.CRM.Models;

namespace Quirke.CRM.Services
{
    public interface IEmployeeService
    {
        // Adds a new employee
        Task<int> AddEmployeeAsync(EmployeeModel employee);

        // Retrieves an employee by their ID
        Task<EmployeeModel> GetEmployeeByIdAsync(int id);

        // Retrieves all employees
        Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync();

        // Updates an existing employee
        Task<bool> UpdateEmployeeAsync(EmployeeModel employee);

        // Deletes an employee by their ID
        Task<bool> DeleteEmployeeAsync(int id);

        // Retrieves employees by a specific job title
        Task<IEnumerable<EmployeeModel>> GetEmployeesByJobTitleAsync(string jobTitle);

        // Retrieves employees hired after a certain date
        Task<IEnumerable<EmployeeModel>> GetEmployeesHiredAfterDateAsync(DateTime hireDate);

        // Checks if an employee with a specific email exists
        Task<bool> EmployeeExistsAsync(string email);
    }
}
