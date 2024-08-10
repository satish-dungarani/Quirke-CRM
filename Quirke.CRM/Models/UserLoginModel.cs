using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class UserLoginModel
    {
        public bool RememberMe { get; set; }

        [EmailAddress(ErrorMessage = "Email address is not in the correct format.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
