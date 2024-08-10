using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class EmployeeLeaveModel
    {
        public EmployeeLeaveModel()
        {
            LeaveTypeList = new List<SelectListItem>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee is required.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Leave Type is required.")]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

        [Range(0, 99.99, ErrorMessage = "Available Leave must be between 0 and 99.99.")]
        [Display(Name = "Available Leave")]
        public decimal AvailableLeave { get; set; }

        [Range(0, 99.99, ErrorMessage = "Pending Leave must be between 0 and 99.99.")]
        [Display(Name = "Pending Leave")]
        public decimal PendingLeave { get; set; }

        [Required]
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Display(Name = "Updated On")]
        public DateTime? UpdatedOn { get; set; }

        public  List<SelectListItem> LeaveTypeList { get; set; }
        public string LeaveType { get; set; }
    }
}
