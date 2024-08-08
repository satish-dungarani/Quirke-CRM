namespace Quirke.CRM.Domain
{
    public class CustomerCompliance
    {
        public int Id { get; set; } // Primary key
        public int CustomerId { get; set; } // Foreign key or reference to Customer
       
        public bool IsAllergicToColour { get; set; } // Nullable (bit)
        public string? AllergicColourDetails { get; set; } // Nullable
        public bool IsDamagedScalp { get; set; } // Nullable (bit)
        public string? ScalpDetails { get; set; } // Nullable
        public bool HasTattoo { get; set; } // Nullable (bit)
        public string? TattooDetails { get; set; } // Nullable
        public bool IsAllergicToProduct { get; set; } // Nullable (bit)
        public string? AllergicProductDetails { get; set; } // Nullable

        public required string Status { get; set; } // Nullable
        public bool CanTakeService { get; set; } // Nullable (bit)
        public bool IsAllergyTestDone { get; set; } // Nullable (bit)
        public DateTime? TestScheduleOn { get; set; } // Nullable
        public DateTime? TestDate { get; set; } // Nullable
        public string? ObservedBy { get; set; } // Nullable

        public DateTime CreatedOn { get; set; } // Nullable
        public DateTime? UpdatedOn { get; set; } // Nullable
    }
}
