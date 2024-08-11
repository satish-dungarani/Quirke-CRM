using Quirke.CRM.Models;

namespace Quirke.CRM.Services
{
    public interface ILeaveRequestService
    {
        Task<IEnumerable<LeaveRequestModel>> GetAllLeaveRequestsAsync();

        Task<LeaveRequestModel> GetLeaveRequestByIdAsync(int id);

        Task<IEnumerable<LeaveRequestModel>> GetLeaveRequestsByEmployeeIdAsync(int employeeId);

        Task<LeaveRequestModel> CreateLeaveRequestAsync(LeaveRequestModel leaveRequestModel);

        Task<LeaveRequestModel> UpdateLeaveRequestAsync(LeaveRequestModel leaveRequestModel);

        Task<bool> DeleteLeaveRequestAsync(int id);

        Task<bool> UpdateLeaveRequestStatusAsync(int id, string status);
    }
}
