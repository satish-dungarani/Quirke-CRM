using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
            SupplierList = new List<SelectListItem>();
            BrandList = new List<SelectListItem>();
        }

        [Display(Name = "Product Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public string? BrandName { get; set; }

        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }
        public string? SupplierName { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity in stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in stock must be a non-negative integer.")]
        [Display(Name = "Quantity In Stock")]
        public int QuantityInStock { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Created On date is required.")]
        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }
        public string? DispCreatedOn { get; set; }

        [Display(Name = "Updated On")]
        public DateTime? UpdatedOn { get; set; }
        public SupplierModel Supplier { get; set; }
        public MasterModel Brand { get; set; }
        public IEnumerable<SelectListItem> SupplierList { get; set; }
        public IEnumerable<SelectListItem> BrandList { get; set; }

    }
}
