namespace Quirke.CRM.Domain
{
    public class Customer
    {
        public int Id { get; set; } // Primary key
        public required string Firstname { get; set; } // Not null
        public required string Lastname { get; set; } // Not null
        public DateTime BirtDate { get; set; } // Not null
        public required string Gender { get; set; } // Nullable
        public required string Mobile { get; set; } // Not null
        public string? Email { get; set; } // Nullable
        public DateTime CreatedOn { get; set; } // Not null
    }
}
