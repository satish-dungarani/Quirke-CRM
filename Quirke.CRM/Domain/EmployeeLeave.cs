namespace Quirke.CRM.Domain
{
    public class EmployeeLeave
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }
        public decimal AvailableLeave { get; set; }
        public decimal PendingLeave { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

        // Navigation properties (optional, based on your ORM or data access strategy)
        public virtual Employee Employee { get; set; }
        public virtual Master LeaveType { get; set; }
    }
}
