using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class CustomerModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string Firstname { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string Lastname { get; set; } = null!;

        [Required(ErrorMessage = "Birth date is required.")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? BirtDate { get; set; }
        public string? DispBirthDate { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public string? Gender { get; set; } = null!;

        [Required(ErrorMessage = "Mobile number is required.")]
        [Display(Name = "Mobile Number")]
        [RegularExpression(@"^07\d{9}$", ErrorMessage = "Please enter a valid UK mobile number.")]
        public string Mobile { get; set; } = null!;

        [Display(Name = "Email Address")]
        [StringLength(50, ErrorMessage = "Email address cannot exceed 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? Email { get; set; } // Nullable

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
        public string? DispCreatedOn { get; set; }
    }
}
