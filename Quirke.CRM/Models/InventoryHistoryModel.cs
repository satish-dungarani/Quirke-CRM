using Quirke.CRM.Domain;
using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Models
{
    public class InventoryHistoryModel
    {
        [Key]
        public int Id { get; set; }  // Primary key

        [Required(ErrorMessage = "Product is required.")]
        [Display(Name = "Product")]
        public int ProductId { get; set; }  // Foreign key referencing Products

        [Required(ErrorMessage = "Quantity in Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity in Stock must be a non-negative number.")]
        [Display(Name = "Quantity in Stock")]
        public int QuantityInStock { get; set; }  // Quantity currently in stock

        [Required(ErrorMessage = "Updated Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Updated Quantity must be a positive number.")]
        [Display(Name = "Updated Quantity")]
        public int UpdatedQuantity { get; set; }  // Quantity that was updated

        [Required(ErrorMessage = "Creation Date is required.")]
        [Display(Name = "Creation Date")]
        public DateTime CreatedOn { get; set; }  // Date and time the record was created

        // Navigation property for the related Product entity (optional, based on your model setup)
        public ProductModel Product { get; set; }
        public string? ProductName { get; set; }


    }
}
