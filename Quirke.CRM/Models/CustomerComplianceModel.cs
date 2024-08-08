using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class CustomerComplianceModel
    {
        [Key]
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
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [Display(Name = "Mobile Number")]
        [StringLength(20, ErrorMessage = "Mobile number cannot exceed 20 characters.")]
        public string Mobile { get; set; } = null!;

        [Display(Name = "Customer Id")]
        public int CustomerId { get; set; }

        [Display(Name = "Is Over 16")]
        public bool IsOver16 => BirthDate.HasValue && DateTime.Now.Year - BirthDate.Value.Year >= 16 &&
                                (DateTime.Now.Month > BirthDate.Value.Month ||
                                DateTime.Now.Month == BirthDate.Value.Month && DateTime.Now.Day >= BirthDate.Value.Day);

        [Display(Name = "Has your client ever had an allergic reaction to hair colour?")]
        public bool IsAllergicToColour { get; set; }

        [Display(Name = "Details")]
        public string? AllergicColourDetails { get; set; }

        [Display(Name = "Has your client got sensitive, irritated, or damaged scalp?")]
        public bool IsDamagedScalp { get; set; }

        [Display(Name = "Details")]
        public string? ScalpDetails { get; set; }

        [Display(Name = "Has your client had any type of skin tattoo, including a temporary black henna tattoo or permanent make-up since their last colour?")]
        public bool HasTattoo { get; set; }

        [Display(Name = "Details")]
        public string? TattooDetails { get; set; }

        [Display(Name = "Has your client had an allergic reaction to any products except hair colour since their last colour?")]
        public bool IsAllergicToProduct { get; set; }

        [Display(Name = "Details")]
        public string? AllergicProductDetails { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Can the Service go ahead?")]
        public bool CanTakeService { get; set; }

        [Display(Name = "Allergy Skin Test done")]
        public bool IsAllergyTestDone { get; set; }

        [Display(Name = "Test Schedule On Date")]
        public DateTime? TestScheduleOn { get; set; }

        [Display(Name = "Date")]
        public DateTime? TestDate { get; set; }

        [Display(Name = "Reaction Observed by client or Salon")]
        public string? ObservedBy { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Updated On")]
        public DateTime? UpdatedOn { get; set; }

        public bool IsValid { get; set; }

    }
}
