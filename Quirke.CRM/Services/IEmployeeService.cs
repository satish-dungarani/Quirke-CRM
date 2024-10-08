﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Quirke.CRM.Models;

namespace Quirke.CRM.Services
{
    public interface IEmployeeService
    {
        Task<int> AddEmployeeAsync(EmployeeModel employee);

        Task<EmployeeModel> GetEmployeeByIdAsync(int id);

        Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync();

        Task<bool> UpdateEmployeeAsync(EmployeeModel employee);

        Task<bool> DeleteEmployeeAsync(int id);

        Task<IEnumerable<EmployeeModel>> GetEmployeesByJobTitleAsync(string jobTitle);

        Task<IEnumerable<EmployeeModel>> GetEmployeesHiredAfterDateAsync(DateTime hireDate);

        Task<bool> EmployeeExistsAsync(string email);

        Task<EmployeeLeaveModel> GetEmployeeLeaveByIdAsync(int id);
        Task<List<EmployeeLeaveModel>> GetEmployeeLeavesByEmployeeIdAsync(int employeeId);
        Task<bool> AddOrUpdateEmployeeLeaveAsync(EmployeeLeaveModel model);
        Task<bool> DeleteEmployeeLeaveAsync(int id);
        Task<bool> IsDuplicateLeaveAsync(int employeeId, int leaveTypeId, int? excludeId = null);
        Task<List<SelectListItem>> GetLeaveTypesForEmployeeAsync(int employeeId);
        Task<decimal> RmainingLeaves(int employeeId, int leaveTypeId);
        Task<IEnumerable<EmployeeModel>> GetAllEmployeesPagingAsync(int page = 1, int pageSize = 10);
    }
}
