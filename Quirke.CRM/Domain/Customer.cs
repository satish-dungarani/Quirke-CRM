namespace Quirke.CRM.Domain
{
    public class Customer
    {
        public int Id { get; set; } // Primary key
        public string Firstname { get; set; } // Not null
        public string Lastname { get; set; } // Not null
        public DateTime BirtDate { get; set; } // Not null
        public string Gender { get; set; } // Nullable
        public string Mobile { get; set; } // Not null
        public string? Email { get; set; } // Nullable
        public DateTime CreatedOn { get; set; } // Not null
    }
}
