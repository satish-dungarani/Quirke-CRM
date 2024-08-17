using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class LeaveRequestModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee is required.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Leave Type is required.")]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }

        [Required(ErrorMessage = "Start Date is required.")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        public string? DispStartDate { get; set; }

        [Required(ErrorMessage = "Leave Duration is required.")]
        [Display(Name = "Leave Duration")]
        [StringLength(50, ErrorMessage = "Leave Duration cannot exceed 50 characters.")]
        public string LeaveDuration { get; set; }

        [Required(ErrorMessage = "End Date is required.")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public string? DispEndDate { get; set; }

        [Display(Name = "Note")]
        [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters.")]
        public string Note { get; set; }

        [Display(Name = "Document")]
        public string Document { get; set; }

        [Required(ErrorMessage = "Request Status is required.")]
        [Display(Name = "Request Status")]
        [StringLength(30, ErrorMessage = "Request Status cannot exceed 30 characters.")]
        public string RequestStatus { get; set; }

        [Required(ErrorMessage = "Created On is required.")]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }
        public string? DispCreatedOn { get; set; }

        [Display(Name = "Updated On")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdateOn { get; set; }

        public EmployeeModel Employee { get; set; }

        public MasterModel Master { get; set; }

        public List<SelectListItem> EmployeeList { get; set; }
        public List<SelectListItem> LeaveTypeList { get; set; }

        public string LeaveType { get; set; }
        public string EmployeeName { get; set; }

        public decimal? AppliedDays { get; set; }
    }

}
