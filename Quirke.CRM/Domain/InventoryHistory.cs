namespace Quirke.CRM.Domain
{
    public class InventoryHistory
    {
        public int Id { get; set; }  // Primary key
        public int ProductId { get; set; }  // Foreign key referencing Products
        public int QuantityInStock { get; set; }  // Quantity currently in stock
        public int UpdatedQuantity { get; set; }  // Quantity that was updated
        public DateTime CreatedOn { get; set; }  // Date and time the record was created

        // Navigation property for the related Product entity (optional, based on your model setup)
        public Product Product { get; set; }
    }
}
