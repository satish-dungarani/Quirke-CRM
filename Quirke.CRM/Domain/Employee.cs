namespace Quirke.CRM.Domain
{
    public class Employee
    {
        public int Id { get; set; } // Primary key
        public string Firstname { get; set; } // Not null
        public string Lastname { get; set; } // Not null
        public string Gender { get; set; } // Nullable
        public DateTime BirthDate { get; set; } // Not null
        public string PhoneNumber { get; set; } // Not null
        public string? EmergencyContact { get; set; } // Not null
        public string? Email { get; set; } // Nullable
        public DateTime HireDate { get; set; } // Not null
        public string JobTitle { get; set; } // Not null
        public string? Picture { get; set; } // Nullable
        public string? IdentityDocument { get; set; } // Nullable
        public bool IsDeleted { get; set; } // Not Null
        public DateTime CreatedOn { get; set; } // Not null
        public DateTime? UpdatedOn { get; set; } // Nullable

    }
}
