using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class CustomerModel
    {
        public int Id { get; set; } // Primary key

        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string Firstname { get; set; } = null!; // Not null

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string Lastname { get; set; } = null!; // Not null

        [Required(ErrorMessage = "Birth date is required.")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? BirtDate { get; set; } // Not null

        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = null!; // Not null

        [Required(ErrorMessage = "Mobile number is required.")]
        [Display(Name = "Mobile Number")]
        [StringLength(20, ErrorMessage = "Mobile number cannot exceed 20 characters.")]
        public string Mobile { get; set; } = null!; // Not null

        [Display(Name = "Email Address")]
        [StringLength(50, ErrorMessage = "Email address cannot exceed 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string? Email { get; set; } // Nullable

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; } // Not null
    }
}
