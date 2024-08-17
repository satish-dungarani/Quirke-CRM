using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class SupplierModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Supplier Name")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Contact Name cannot exceed 100 characters.")]
        [Display(Name = "Contact Name")]
        public string? ContactName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [StringLength(100, ErrorMessage = "Contact Email cannot exceed 100 characters.")]
        [Display(Name = "Contact Email")]
        public string? ContactEmail { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number.")]
        [StringLength(20, ErrorMessage = "Contact Phone cannot exceed 20 characters.")]
        [Display(Name = "Contact Phone")]
        public string? ContactPhone { get; set; }

        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters.")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Created On is required.")]
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
        public string? DispCreatedOn { get; set; }

        [Display(Name = "Updated On")]
        public DateTime? UpdatedOn { get; set; }
    }
}
