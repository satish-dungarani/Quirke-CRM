﻿using Microsoft.EntityFrameworkCore;
using Quirke.CRM.DataContext;
using Quirke.CRM.Models;
using Quirke.CRM.Services;

namespace Quirke.CRM.Domain.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveRequestModel>> GetAllLeaveRequestsAsync()
        {
                var leaveRequests = from leaveRequest in _context.LeaveRequests
                                    join employee in _context.Employees on leaveRequest.EmployeeId equals employee.Id
                                    join master in _context.Masters on leaveRequest.LeaveTypeId equals master.Id
                                    select new LeaveRequestModel
                                    {
                                        Id = leaveRequest.Id,
                                        EmployeeId = leaveRequest.EmployeeId,
                                        LeaveTypeId = leaveRequest.LeaveTypeId,
                                        StartDate = leaveRequest.StartDate,
                                        DispStartDate = leaveRequest.StartDate.ToString("dd MMM yyyy"),
                                        EndDate = leaveRequest.EndDate,
                                        DispEndDate = leaveRequest.EndDate.ToString("dd MMM yyyy"),
                                        LeaveDuration = leaveRequest.LeaveDuration,
                                        Note = leaveRequest.Note ?? string.Empty,
                                        Document = leaveRequest.Document ?? string.Empty,
                                        RequestStatus = leaveRequest.RequestStatus,
                                        CreatedOn = leaveRequest.CreatedOn,
                                        DispCreatedOn = leaveRequest.CreatedOn.ToString("dd MMM yyyy"),
                                        UpdateOn = leaveRequest.UpdateOn,
                                        Employee = new EmployeeModel
                                        {
                                            Id = employee.Id,
                                            Firstname = employee.Firstname,
                                            Lastname = employee.Lastname,
                                            Gender = employee.Gender ?? string.Empty,
                                            BirthDate = employee.BirthDate,
                                            DispBirthDate = employee.BirthDate.ToString("dd MMM yyyy"),
                                            PhoneNumber = employee.PhoneNumber,
                                            EmergencyContact = employee.EmergencyContact,
                                            Email = employee.Email ?? string.Empty,
                                            HireDate = employee.HireDate,
                                            DispHireDate = employee.HireDate.ToString("dd MMM yyyy"),
                                            JobTitle = employee.JobTitle,
                                            Picture = employee.Picture ?? string.Empty,
                                            IdentityDocument = employee.IdentityDocument ?? string.Empty,
                                            IsDeleted = employee.IsDeleted,
                                            CreatedOn = employee.CreatedOn,
                                            DispCreatedOn = employee.CreatedOn.ToString("dd MMM yyyy"),
                                            UpdatedOn = employee.UpdatedOn
                                        },
                                        Master = new MasterModel
                                        {
                                            Id = master.Id,
                                            Name = master.Name ?? string.Empty,
                                            MasterTypeId = master.MasterTypeId,
                                            IsDeleted = master.IsDeleted,
                                            CreatedOn = master.CreatedOn,
                                            UpdatedOn = master.UpdatedOn
                                        },
                                        EmployeeName = (employee.Firstname + " " + employee.Lastname) ?? string.Empty,
                                        LeaveType = master.Name ?? string.Empty
                                    };

                return await leaveRequests
                    .OrderByDescending(c => c.Id)
                    .ToListAsync();
            
        }

        public async Task<LeaveRequestModel> GetLeaveRequestByIdAsync(int id)
        {
            var request = await (from leaveRequest in _context.LeaveRequests
                                 join employee in _context.Employees on leaveRequest.EmployeeId equals employee.Id
                                 join master in _context.Masters on leaveRequest.LeaveTypeId equals master.Id
                                 select new LeaveRequestModel
                                 {
                                     Id = leaveRequest.Id,
                                     EmployeeId = leaveRequest.EmployeeId,
                                     LeaveTypeId = leaveRequest.LeaveTypeId,
                                     StartDate = leaveRequest.StartDate,
                                     DispStartDate = leaveRequest.StartDate.ToString("dd MMM yyyy"),
                                     EndDate = leaveRequest.EndDate,
                                     DispEndDate = leaveRequest.EndDate.ToString("dd MMM yyyy"),
                                     LeaveDuration = leaveRequest.LeaveDuration,
                                     Note = leaveRequest.Note ?? string.Empty,
                                     Document = leaveRequest.Document ?? string.Empty,
                                     RequestStatus = leaveRequest.RequestStatus,
                                     CreatedOn = leaveRequest.CreatedOn,
                                     DispCreatedOn = leaveRequest.CreatedOn.ToString("dd MMM yyyy"),
                                     UpdateOn = leaveRequest.UpdateOn,
                                     EmployeeName = (employee.Firstname + " " + employee.Lastname) ?? string.Empty,
                                     LeaveType = master.Name ?? string.Empty
                                 }).FirstOrDefaultAsync(lr => lr.Id == id);

            return request ?? new LeaveRequestModel();

        }

        public async Task<IEnumerable<LeaveRequestModel>> GetLeaveRequestsByEmployeeIdAsync(int employeeId)
        {
            var requests = await (from leaveRequest in _context.LeaveRequests
                                  join employee in _context.Employees on leaveRequest.EmployeeId equals employee.Id
                                  join master in _context.Masters on leaveRequest.LeaveTypeId equals master.Id
                                  select new LeaveRequestModel
                                  {
                                      Id = leaveRequest.Id,
                                      EmployeeId = leaveRequest.EmployeeId,
                                      LeaveTypeId = leaveRequest.LeaveTypeId,
                                      StartDate = leaveRequest.StartDate,
                                      DispStartDate = leaveRequest.StartDate.ToString("dd MMM yyyy"),
                                      EndDate = leaveRequest.EndDate,
                                      DispEndDate = leaveRequest.EndDate.ToString("dd MMM yyyy"),
                                      LeaveDuration = leaveRequest.LeaveDuration,
                                      Note = leaveRequest.Note ?? string.Empty,
                                      Document = leaveRequest.Document ?? string.Empty,
                                      RequestStatus = leaveRequest.RequestStatus,
                                      CreatedOn = leaveRequest.CreatedOn,
                                      DispCreatedOn = leaveRequest.CreatedOn.ToString("dd MMM yyyy"),
                                      UpdateOn = leaveRequest.UpdateOn,
                                      EmployeeName = (employee.Firstname + " " + employee.Lastname) ?? string.Empty,
                                      LeaveType = master.Name ?? string.Empty
                                  }).Where(lr => lr.EmployeeId == employeeId)
                                  .OrderByDescending(c => c.Id)
                                 .ToListAsync();

            return requests;
        }

        public async Task<LeaveRequestModel> CreateLeaveRequestAsync(LeaveRequestModel leaveRequestModel)
        {
            var lr = new LeaveRequest()
            {
                Id = leaveRequestModel.Id,
                EmployeeId = leaveRequestModel.EmployeeId,
                LeaveTypeId = leaveRequestModel.LeaveTypeId,
                StartDate = leaveRequestModel.StartDate.Value,
                EndDate = leaveRequestModel.EndDate.Value,
                LeaveDuration = leaveRequestModel.LeaveDuration,
                RequestStatus = leaveRequestModel.RequestStatus,
                CreatedOn = DateTime.UtcNow,
                Note = leaveRequestModel.Note,
                Document = leaveRequestModel.Document
            };

            _context.LeaveRequests.Add(lr);
            await _context.SaveChangesAsync();
            leaveRequestModel.Id = lr.Id;

            var empLeave = await (from e in _context.EmployeeLeaves
                                  where e.EmployeeId == leaveRequestModel.EmployeeId
                                  && e.LeaveTypeId == leaveRequestModel.LeaveTypeId
                                  select e).FirstOrDefaultAsync();

            if (empLeave != null && leaveRequestModel.RequestStatus.Equals("Approved", StringComparison.CurrentCultureIgnoreCase))
            {
                empLeave.PendingLeave -= Convert.ToDecimal(leaveRequestModel.AppliedDays);
                _context.EmployeeLeaves.Update(empLeave);
                await _context.SaveChangesAsync();
            }

            return leaveRequestModel;
        }

        public async Task<LeaveRequestModel> UpdateLeaveRequestAsync(LeaveRequestModel leaveRequestModel)
        {
            var lr = new LeaveRequest()
            {
                Id = leaveRequestModel.Id,
                EmployeeId = leaveRequestModel.EmployeeId,
                LeaveTypeId = leaveRequestModel.LeaveTypeId,
                StartDate = leaveRequestModel.StartDate.Value,
                EndDate = leaveRequestModel.EndDate.Value,
                LeaveDuration = leaveRequestModel.LeaveDuration,
                RequestStatus = leaveRequestModel.RequestStatus,
                CreatedOn = DateTime.UtcNow,
                Note = leaveRequestModel.Note,
                Document = leaveRequestModel.Document
            };

            _context.LeaveRequests.Update(lr);
            await _context.SaveChangesAsync();
            return leaveRequestModel;
        }

        public async Task<bool> DeleteLeaveRequestAsync(int id)
        {
            try
            {
                var leaveRequest = await _context.LeaveRequests.FindAsync(id);
                if (leaveRequest == null) return false;

                _context.LeaveRequests.Remove(leaveRequest);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> UpdateLeaveRequestStatusAsync(int id, string status)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null) return false;

            leaveRequest.RequestStatus = status;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
