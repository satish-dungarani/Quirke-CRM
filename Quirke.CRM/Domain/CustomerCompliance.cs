namespace Quirke.CRM.Domain
{
    public class CustomerCompliance
    {
        public int Id { get; set; } 
        public int CustomerId { get; set; } 
        public bool IsIdentityProvided { get; set; }
        public bool IsAllergicToColour { get; set; } 
        public string? AllergicColourDetails { get; set; } 
        public bool IsDamagedScalp { get; set; } 
        public string? ScalpDetails { get; set; } 
        public bool HasTattoo { get; set; } 
        public string? TattooDetails { get; set; } 
        public bool IsAllergicToProduct { get; set; } 
        public string? AllergicProductDetails { get; set; } 

        public string Status { get; set; } 
        public bool CanTakeService { get; set; } 
        public bool IsAllergyTestDone { get; set; } 
        public DateTime? TestScheduleOn { get; set; } 
        public DateTime? TestDate { get; set; } 
        public string? ObservedBy { get; set; } 

        public DateTime CreatedOn { get; set; } 
        public DateTime? UpdatedOn { get; set; } 
        public string SignatureData { get; set; } 
    }
}
