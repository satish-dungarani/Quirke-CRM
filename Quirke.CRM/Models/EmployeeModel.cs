using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class EmployeeModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
        [Display(Name = "First Name")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
        [Display(Name = "Last Name")]
        public string? Lastname { get; set; }

        [StringLength(10, ErrorMessage = "Gender cannot be longer than 10 characters")]
        [Display(Name = "Gender")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Birth date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }
        public string? DispBirthDate { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^07\d{9}$", ErrorMessage = "Please enter a valid UK mobile number.")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Emergency contact is required")]
        [StringLength(50, ErrorMessage = "Emergency contact cannot be longer than 50 characters")]
        [Display(Name = "Emergency Contact")]
        public string? EmergencyContact { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Hire date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime? HireDate { get; set; }
        public string? DispHireDate { get; set; }

        [Required(ErrorMessage = "Job title is required")]
        [StringLength(100, ErrorMessage = "Job title cannot be longer than 100 characters")]
        [Display(Name = "Job Title")]
        public string? JobTitle { get; set; }

        [Display(Name = "Picture")]
        public string? Picture { get; set; }

        [Display(Name = "Identity Document")]
        public string? IdentityDocument { get; set; }

        [Required(ErrorMessage = "IsDeleted flag is required")]
        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Creation date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
        public string? DispCreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Updated On")]
        public DateTime? UpdatedOn { get; set; }
    }
}
