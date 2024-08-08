using Microsoft.EntityFrameworkCore;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain;
using Quirke.CRM.Models;

namespace Quirke.CRM.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddEmployeeAsync(EmployeeModel employee)
        {
            var entity = new Employee
            {
                Firstname = employee.Firstname,
                Lastname = employee.Lastname,
                Gender = employee.Gender,
                BirthDate = employee.BirthDate,
                PhoneNumber = employee.PhoneNumber,
                EmergencyContact = employee.EmergencyContact,
                Email = employee.Email,
                HireDate = employee.HireDate,
                JobTitle = employee.JobTitle,
                Picture = employee.Picture,
                IdentityDocument = employee.IdentityDocument,
                IsDeleted = false,
                CreatedOn = DateTime.UtcNow
            };

            _context.Employees.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<EmployeeModel> GetEmployeeByIdAsync(int id)
        {
            var entity = await _context.Employees.FindAsync(id);
            if (entity == null || entity.IsDeleted)
            {
                return null;
            }

            return new EmployeeModel
            {
                Id = entity.Id,
                Firstname = entity.Firstname,
                Lastname = entity.Lastname,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                PhoneNumber = entity.PhoneNumber,
                EmergencyContact = entity.EmergencyContact,
                Email = entity.Email,
                HireDate = entity.HireDate,
                JobTitle = entity.JobTitle,
                Picture = entity.Picture,
                IdentityDocument = entity.IdentityDocument,
                IsDeleted = entity.IsDeleted,
                CreatedOn = entity.CreatedOn,
                UpdatedOn = entity.UpdatedOn
            };
        }

        public async Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync()
        {
            try
            {
                return await _context.Employees
               .Where(e => !e.IsDeleted)
               .Select(e => new EmployeeModel
               {
                   Id = e.Id,
                   Firstname = e.Firstname,
                   Lastname = e.Lastname,
                   Gender = e.Gender,
                   BirthDate = e.BirthDate,
                   PhoneNumber = e.PhoneNumber,
                   EmergencyContact = e.EmergencyContact,
                   Email = e.Email,
                   HireDate = e.HireDate,
                   JobTitle = e.JobTitle,
                   Picture = e.Picture,
                   IdentityDocument = e.IdentityDocument,
                   IsDeleted = e.IsDeleted,
                   CreatedOn = e.CreatedOn,
                   UpdatedOn = e.UpdatedOn
               })
               .ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeModel employee)
        {
            var entity = await _context.Employees.FindAsync(employee.Id);
            if (entity == null || entity.IsDeleted)
            {
                return false;
            }

            entity.Firstname = employee.Firstname;
            entity.Lastname = employee.Lastname;
            entity.Gender = employee.Gender;
            entity.BirthDate = employee.BirthDate;
            entity.PhoneNumber = employee.PhoneNumber;
            entity.EmergencyContact = employee.EmergencyContact;
            entity.Email = employee.Email;
            entity.HireDate = employee.HireDate;
            entity.JobTitle = employee.JobTitle;
            entity.Picture = employee.Picture;
            entity.IdentityDocument = employee.IdentityDocument;
            entity.UpdatedOn = DateTime.UtcNow;

            _context.Employees.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var entity = await _context.Employees.FindAsync(id);
            if (entity == null || entity.IsDeleted)
            {
                return false;
            }

            // Soft delete: set IsDeleted flag
            entity.IsDeleted = true;
            entity.UpdatedOn = DateTime.UtcNow;

            _context.Employees.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployeesByJobTitleAsync(string jobTitle)
        {
            return await _context.Employees
                .Where(e => e.JobTitle == jobTitle && !e.IsDeleted)
                .Select(e => new EmployeeModel
                {
                    Id = e.Id,
                    Firstname = e.Firstname,
                    Lastname = e.Lastname,
                    Gender = e.Gender,
                    BirthDate = e.BirthDate,
                    PhoneNumber = e.PhoneNumber,
                    EmergencyContact = e.EmergencyContact,
                    Email = e.Email,
                    HireDate = e.HireDate,
                    JobTitle = e.JobTitle,
                    Picture = e.Picture,
                    IdentityDocument = e.IdentityDocument,
                    IsDeleted = e.IsDeleted,
                    CreatedOn = e.CreatedOn,
                    UpdatedOn = e.UpdatedOn
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployeesHiredAfterDateAsync(DateTime hireDate)
        {
            return await _context.Employees
                .Where(e => e.HireDate > hireDate && !e.IsDeleted)
                .Select(e => new EmployeeModel
                {
                    Id = e.Id,
                    Firstname = e.Firstname,
                    Lastname = e.Lastname,
                    Gender = e.Gender,
                    BirthDate = e.BirthDate,
                    PhoneNumber = e.PhoneNumber,
                    EmergencyContact = e.EmergencyContact,
                    Email = e.Email,
                    HireDate = e.HireDate,
                    JobTitle = e.JobTitle,
                    Picture = e.Picture,
                    IdentityDocument = e.IdentityDocument,
                    IsDeleted = e.IsDeleted,
                    CreatedOn = e.CreatedOn,
                    UpdatedOn = e.UpdatedOn
                })
                .ToListAsync();
        }

        public async Task<bool> EmployeeExistsAsync(string email)
        {
            return await _context.Employees
                .AnyAsync(e => e.Email == email && !e.IsDeleted);
        }

    }
}
