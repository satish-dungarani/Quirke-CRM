using Quirke.CRM.Domain;
using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class InventoryHistoryModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Product is required.")]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Quantity in Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in Stock must be a non-negative number.")]
        [Display(Name = "Quantity in Stock")]
        public int QuantityInStock { get; set; } 

        [Required(ErrorMessage = "Updated Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Updated Quantity must be a positive number.")]
        [Display(Name = "Updated Quantity")]
        public int UpdatedQuantity { get; set; }

        [Required(ErrorMessage = "Creation Date is required.")]
        [Display(Name = "Creation Date")]
        public DateTime CreatedOn { get; set; }
        public string? DispCreatedOn { get; set; }

        public ProductModel Product { get; set; }
        public string? ProductName { get; set; }


    }
}
