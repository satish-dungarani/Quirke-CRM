using Microsoft.AspNetCore.Mvc.Rendering;
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
                BirthDate = employee.BirthDate.Value,
                PhoneNumber = employee.PhoneNumber,
                EmergencyContact = employee.EmergencyContact,
                Email = employee.Email,
                HireDate = employee.HireDate.Value,
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
                DispBirthDate = entity.BirthDate.ToString("dd MMM yyyy"),
                PhoneNumber = entity.PhoneNumber,
                EmergencyContact = entity.EmergencyContact,
                Email = entity.Email,
                HireDate = entity.HireDate,
                DispHireDate = entity.HireDate.ToString("dd MMM yyyy"),
                JobTitle = entity.JobTitle,
                Picture = entity.Picture,
                IdentityDocument = entity.IdentityDocument,
                IsDeleted = entity.IsDeleted,
                CreatedOn = entity.CreatedOn,
                DispCreatedOn = entity.CreatedOn.ToString("dd MMM yyyy"),
                UpdatedOn = entity.UpdatedOn
            };
        }

        public async Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync()
        {
            return await _context.Employees
           .Where(e => !e.IsDeleted)
           .Select(entity => new EmployeeModel
           {
               Id = entity.Id,
               Firstname = entity.Firstname,
               Lastname = entity.Lastname,
               Gender = entity.Gender,
               BirthDate = entity.BirthDate,
               DispBirthDate = entity.BirthDate.ToString("dd MMM yyyy"),
               PhoneNumber = entity.PhoneNumber,
               EmergencyContact = entity.EmergencyContact,
               Email = entity.Email,
               HireDate = entity.HireDate,
               DispHireDate = entity.HireDate.ToString("dd MMM yyyy"),
               JobTitle = entity.JobTitle,
               Picture = entity.Picture,
               IdentityDocument = entity.IdentityDocument,
               IsDeleted = entity.IsDeleted,
               CreatedOn = entity.CreatedOn,
               DispCreatedOn = entity.CreatedOn.ToString("dd MMM yyyy"),
               UpdatedOn = entity.UpdatedOn
           }).OrderByDescending(c => c.Id).ToListAsync();
        }

        public async Task<IEnumerable<EmployeeModel>> GetAllEmployeesPagingAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Employees
           .Where(e => !e.IsDeleted)
           .OrderByDescending(c => c.Id)
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .Select(entity => new EmployeeModel
           {
               Id = entity.Id,
               Firstname = entity.Firstname,
               Lastname = entity.Lastname,
               Gender = entity.Gender,
               BirthDate = entity.BirthDate,
               DispBirthDate = entity.BirthDate.ToString("dd MMM yyyy"),
               PhoneNumber = entity.PhoneNumber,
               EmergencyContact = entity.EmergencyContact,
               Email = entity.Email,
               HireDate = entity.HireDate,
               DispHireDate = entity.HireDate.ToString("dd MMM yyyy"),
               JobTitle = entity.JobTitle,
               Picture = entity.Picture,
               IdentityDocument = entity.IdentityDocument,
               IsDeleted = entity.IsDeleted,
               CreatedOn = entity.CreatedOn,
               DispCreatedOn = entity.CreatedOn.ToString("dd MMM yyyy"),
               UpdatedOn = entity.UpdatedOn
           })
           .ToListAsync();
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
            entity.BirthDate = employee.BirthDate.Value;
            entity.PhoneNumber = employee.PhoneNumber;
            entity.EmergencyContact = employee.EmergencyContact;
            entity.Email = employee.Email;
            entity.HireDate = employee.HireDate.Value;
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
                .Select(entity => new EmployeeModel
                {
                    Id = entity.Id,
                    Firstname = entity.Firstname,
                    Lastname = entity.Lastname,
                    Gender = entity.Gender,
                    BirthDate = entity.BirthDate,
                    DispBirthDate = entity.BirthDate.ToString("dd MMM yyyy"),
                    PhoneNumber = entity.PhoneNumber,
                    EmergencyContact = entity.EmergencyContact,
                    Email = entity.Email,
                    HireDate = entity.HireDate,
                    DispHireDate = entity.HireDate.ToString("dd MMM yyyy"),
                    JobTitle = entity.JobTitle,
                    Picture = entity.Picture,
                    IdentityDocument = entity.IdentityDocument,
                    IsDeleted = entity.IsDeleted,
                    CreatedOn = entity.CreatedOn,
                    DispCreatedOn = entity.CreatedOn.ToString("dd MMM yyyy"),
                    UpdatedOn = entity.UpdatedOn
                })
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeModel>> GetEmployeesHiredAfterDateAsync(DateTime hireDate)
        {
            return await _context.Employees
                .Where(e => e.HireDate > hireDate && !e.IsDeleted)
                .Select(entity => new EmployeeModel
                {
                    Id = entity.Id,
                    Firstname = entity.Firstname,
                    Lastname = entity.Lastname,
                    Gender = entity.Gender,
                    BirthDate = entity.BirthDate,
                    DispBirthDate = entity.BirthDate.ToString("dd MMM yyyy"),
                    PhoneNumber = entity.PhoneNumber,
                    EmergencyContact = entity.EmergencyContact,
                    Email = entity.Email,
                    HireDate = entity.HireDate,
                    DispHireDate = entity.HireDate.ToString("dd MMM yyyy"),
                    JobTitle = entity.JobTitle,
                    Picture = entity.Picture,
                    IdentityDocument = entity.IdentityDocument,
                    IsDeleted = entity.IsDeleted,
                    CreatedOn = entity.CreatedOn,
                    DispCreatedOn = entity.CreatedOn.ToString("dd MMM yyyy"),
                    UpdatedOn = entity.UpdatedOn
                })
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<bool> EmployeeExistsAsync(string email)
        {
            return await _context.Employees
                .AnyAsync(e => e.Email == email && !e.IsDeleted);
        }

        public async Task<EmployeeLeaveModel> GetEmployeeLeaveByIdAsync(int id)
        {
            var leave = await _context.EmployeeLeaves
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EmployeeLeaveModel
                {
                    Id = e.Id,
                    EmployeeId = e.EmployeeId,
                    LeaveTypeId = e.LeaveTypeId,
                    AvailableLeave = e.AvailableLeave,
                    PendingLeave = e.PendingLeave,
                    CreatedOn = e.CreatedOn,
                    UpdatedOn = e.UpdatedOn
                })
                .FirstOrDefaultAsync();

            return leave;
        }

        public async Task<List<EmployeeLeaveModel>> GetEmployeeLeavesByEmployeeIdAsync(int employeeId)
        {
            return await _context.EmployeeLeaves
                 .AsNoTracking()
                 .Where(e => e.EmployeeId == employeeId)
                 .Join(
                     _context.Masters,
                     leave => leave.LeaveTypeId,
                     master => master.Id,
                     (leave, master) => new EmployeeLeaveModel
                     {
                         Id = leave.Id,
                         EmployeeId = leave.EmployeeId,
                         LeaveTypeId = leave.LeaveTypeId,
                         LeaveType = master.Name,
                         AvailableLeave = leave.AvailableLeave,
                         PendingLeave = leave.PendingLeave,
                         CreatedOn = leave.CreatedOn,
                         UpdatedOn = leave.UpdatedOn
                     }
                 )
                 .OrderByDescending(c => c.Id)
                 .ToListAsync();
        }

        public async Task<bool> AddOrUpdateEmployeeLeaveAsync(EmployeeLeaveModel model)
        {
            if (model.Id > 0)
            {
                var leave = await _context.EmployeeLeaves.FindAsync(model.Id);
                if (leave == null) return false;

                leave.EmployeeId = model.EmployeeId;
                leave.LeaveTypeId = model.LeaveTypeId;
                leave.AvailableLeave = model.AvailableLeave;
                //leave.PendingLeave = model.PendingLeave;
                leave.UpdatedOn = DateTime.Now;

                _context.EmployeeLeaves.Update(leave);
            }
            else
            {
                var leave = new EmployeeLeave
                {
                    EmployeeId = model.EmployeeId,
                    LeaveTypeId = model.LeaveTypeId,
                    AvailableLeave = model.AvailableLeave,
                    PendingLeave = model.AvailableLeave,
                    CreatedOn = DateTime.Now
                };

                await _context.EmployeeLeaves.AddAsync(leave);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsDuplicateLeaveAsync(int employeeId, int leaveTypeId, int? excludeId = null)
        {
            return await _context.EmployeeLeaves
                .AnyAsync(e => e.EmployeeId == employeeId
                            && e.LeaveTypeId == leaveTypeId
                            && (excludeId == null || e.Id != excludeId));
        }

        public async Task<bool> DeleteEmployeeLeaveAsync(int id)
        {
            var leave = await _context.EmployeeLeaves.FindAsync(id);
            if (leave == null) return false;

            _context.EmployeeLeaves.Remove(leave);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SelectListItem>> GetLeaveTypesForEmployeeAsync(int employeeId)
        {
            return await _context.EmployeeLeaves
                .Where(el => el.EmployeeId == employeeId && el.PendingLeave > 0)
                .Join(
                    _context.Masters,
                    el => el.LeaveTypeId,
                    m => m.Id,
                    (el, m) => new SelectListItem
                    {
                        Value = m.Id.ToString(),
                        Text = m.Name,
                    }
                ).ToListAsync();

        }

        public async Task<decimal> RmainingLeaves(int employeeId, int leaveTypeId)
        {
            var empLeave = await _context.EmployeeLeaves
                .Where(el => el.EmployeeId == employeeId && el.LeaveTypeId == leaveTypeId)
                .FirstOrDefaultAsync();

            return empLeave == null ? 0 : empLeave.PendingLeave;
        }
    }
}
