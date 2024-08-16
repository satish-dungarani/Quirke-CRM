using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Quirke.CRM.Models
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [Display(Name = "Username")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        [Display(Name = "First Name")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [Display(Name = "Last Name")]
        public required string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone Number is required.")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Email address is not in the correct format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [PasswordComplexity]
        public required string Password { get; set; }


    }
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        private const int MinimumLength = 6;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Password is required.");
            }

            var password = value.ToString();
            var errorMessages = new StringBuilder();

            // Always include all the rules, even if some are met.
            if (password.Length < MinimumLength)
            {
                errorMessages.AppendLine("• Password must be at least 6 characters long.");
            }
            else
            {
                errorMessages.AppendLine("• Password is at least 6 characters long.");
            }

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                errorMessages.AppendLine("• Password must contain at least one non-alphanumeric character.");
            }
            else
            {
                errorMessages.AppendLine("• Password contains at least one non-alphanumeric character.");
            }

            if (!password.Any(char.IsDigit))
            {
                errorMessages.AppendLine("• Password must contain at least one digit ('0'-'9').");
            }
            else
            {
                errorMessages.AppendLine("• Password contains at least one digit ('0'-'9').");
            }

            if (!password.Any(char.IsUpper))
            {
                errorMessages.AppendLine("• Password must contain at least one uppercase letter ('A'-'Z').");
            }
            else
            {
                errorMessages.AppendLine("• Password contains at least one uppercase letter ('A'-'Z').");
            }

            // Return the aggregated error messages if any rule is violated.
            if (errorMessages.ToString().Contains("• Password must"))
            {
                return new ValidationResult(errorMessages.ToString().TrimEnd());
            }

            return ValidationResult.Success;
        }
    }
}
