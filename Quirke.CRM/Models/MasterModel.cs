using System.ComponentModel.DataAnnotations;

public class MasterModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "MasterTypeId is required.")]
    [Display(Name = "Master Type ID")]
    public int MasterTypeId { get; set; }

    [Display(Name = "Is Deleted")]
    public bool IsDeleted { get; set; } = false;

    [Required(ErrorMessage = "Created On is required.")]
    [Display(Name = "Created On")]
    public DateTime CreatedOn { get; set; }

    [Display(Name = "Updated On")]
    public DateTime? UpdatedOn { get; set; }
}