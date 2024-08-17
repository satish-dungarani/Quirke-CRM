using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class CustomerRecordModel
    {
        public CustomerRecordModel()
        {
            ProductList = new List<SelectListItem>();
            TreatmentList = new List<SelectListItem>();
            AttendedEmployeeList = new List<SelectListItem>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Customer is required.")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Product is required.")]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Treatment is required.")]
        [Display(Name = "Treatment")]
        public int TreatmentId { get; set; }

        [Required(ErrorMessage = "Service Date is required.")]
        [Display(Name = "Service Date")]
        [DataType(DataType.Date)]
        public DateTime ServiceDate { get; set; }
        public string? DispServiceDate { get; set; }

        [StringLength(100, ErrorMessage = "Strength cannot exceed 100 characters.")]
        [Display(Name = "Strength")]
        public string? Strength { get; set; }

        [Display(Name = "Development Time")]
        public string? DevTime { get; set; }

        [StringLength(500, ErrorMessage = "Remark cannot exceed 500 characters.")]
        [Display(Name = "Remark")]
        public string? Remark { get; set; }

        [Display(Name = "Attended Employee")]
        public int? AttendedEmployeeId { get; set; }

        [Required(ErrorMessage = "Created On date is required.")]
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
        public string? DispCreatedOn { get; set; }

        [Display(Name = "Updated On")]
        public DateTime? UpdatedOn { get; set; }

        public string? ProductName { get; set; }
        public string? TreatmentName { get; set; }
        public string? AttendedEmployeeName { get; set; }

        public IEnumerable<SelectListItem> ProductList { get; set; }
        public IEnumerable<SelectListItem> TreatmentList { get; set; }
        public IEnumerable<SelectListItem> AttendedEmployeeList { get; set; }
    }
}
