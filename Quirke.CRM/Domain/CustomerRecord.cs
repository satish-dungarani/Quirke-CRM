using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Quirke.CRM.Domain
{
    public class CustomerRecord
    {
        public int Id { get; set; } // Primary key
        public int CustomerId { get; set; } // Foreign key to Customer
        public int? ProductId { get; set; } // Foreign key to Product
        public int? TreatmentId { get; set; } // Foreign key to Treatment
        public int? AttendedEmployeeId { get; set; } // Nullable foreign key to Employee
        public DateTime ServiceDate { get; set; } // Date and time of the service
        public string Strength { get; set; } // Nullable NVARCHAR(100)
        public string? DevTime { get; set; } // Nullable time duration
        public string? Remark { get; set; } // Nullable NVARCHAR(500)
        public DateTime CreatedOn { get; set; } // Timestamp of creation
        public DateTime? UpdatedOn { get; set; } // Nullable timestamp of last update
    }
}
