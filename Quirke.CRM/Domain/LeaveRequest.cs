namespace Quirke.CRM.Domain
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public int LeaveTypeId { get; set; }

        public DateTime StartDate { get; set; } 

        public string LeaveDuration { get; set; } 

        public DateTime EndDate { get; set; }  

        public string? Note { get; set; }  

        public string? Document { get; set; }

        public string RequestStatus { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now; 

        public DateTime? UpdateOn { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Master LeaveType { get; set; }
    }
}
