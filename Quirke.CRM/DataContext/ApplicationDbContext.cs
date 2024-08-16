using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quirke.CRM.Domain;
using Quirke.CRM.Models;

namespace Quirke.CRM.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerCompliance> CustomerCompliances { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Master> Masters { get; set; }
        public DbSet<EmployeeLeave> EmployeeLeaves { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CustomerRecord> CustomerRecords { get; set; }
        public DbSet<InventoryHistory> InventoryHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Firstname)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(e => e.Lastname)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(e => e.BirtDate)
                      .IsRequired()
                      .HasColumnType("date");
                entity.Property(e => e.Gender)
                      .HasMaxLength(20);
                entity.Property(e => e.Mobile)
                      .IsRequired()
                      .HasMaxLength(20);
                entity.Property(e => e.Email)
                      .HasMaxLength(50);
                entity.Property(e => e.CreatedOn)
                      .IsRequired()
                      .HasColumnType("datetime");
            });

            modelBuilder.Entity<CustomerCompliance>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerId)
                      .IsRequired();
                entity.HasOne<Customer>()
                      .WithMany()
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.IsIdentityProvided)
                      .IsRequired();
                entity.Property(e => e.IsAllergicToColour)
                      .IsRequired();
                entity.Property(e => e.AllergicColourDetails)
                      .HasMaxLength(200)
                      .IsRequired(false);
                entity.Property(e => e.IsDamagedScalp)
                      .IsRequired();
                entity.Property(e => e.ScalpDetails)
                      .HasMaxLength(200)
                      .IsRequired(false);
                entity.Property(e => e.HasTattoo)
                      .IsRequired();
                entity.Property(e => e.TattooDetails)
                      .HasMaxLength(200)
                      .IsRequired(false);
                entity.Property(e => e.IsAllergicToProduct)
                      .IsRequired();
                entity.Property(e => e.AllergicProductDetails)
                      .HasMaxLength(200)
                      .IsRequired(false);
                entity.Property(e => e.Status)
                      .IsRequired();
                entity.Property(e => e.CanTakeService)
                      .IsRequired();
                entity.Property(e => e.IsAllergyTestDone)
                      .IsRequired();
                entity.Property(e => e.TestScheduleOn)
                      .IsRequired(false)
                      .HasColumnType("datetime");
                entity.Property(e => e.TestDate)
                      .IsRequired(false)
                      .HasColumnType("datetime");
                entity.Property(e => e.ObservedBy)
                      .IsRequired(false);
                entity.Property(e => e.SignatureData)
                      .IsRequired(false);
                entity.Property(e => e.CreatedOn)
                      .IsRequired()
                      .HasColumnType("datetime");
                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false)
                      .HasColumnType("datetime");
            });

            modelBuilder.Entity<Employee>(entity =>
              {
                  entity.HasKey(e => e.Id); // Primary key

                  entity.Property(e => e.Firstname)
                      .IsRequired(); // Not null

                  entity.Property(e => e.Lastname)
                      .IsRequired(); // Not null

                  entity.Property(e => e.Gender)
                      .IsRequired(false); // Nullable

                  entity.Property(e => e.BirthDate)
                      .IsRequired(); // Not null

                  entity.Property(e => e.PhoneNumber)
                      .IsRequired(); // Not null

                  entity.Property(e => e.EmergencyContact)
                      .IsRequired(); // Not null

                  entity.Property(e => e.Email)
                      .IsRequired(false); // Nullable

                  entity.Property(e => e.HireDate)
                      .IsRequired(); // Not null

                  entity.Property(e => e.JobTitle)
                      .IsRequired(); // Not null

                  entity.Property(e => e.Picture)
                      .IsRequired(false); // Nullable

                  entity.Property(e => e.IdentityDocument)
                      .IsRequired(false); // Nullable

                  entity.Property(e => e.IsDeleted)
                      .IsRequired(); // Not null

                  entity.Property(e => e.CreatedOn)
                      .IsRequired(); // Not null

                  entity.Property(e => e.UpdatedOn)
                      .IsRequired(false); // Nullable
              });

            modelBuilder.Entity<Master>(entity =>
            {
                entity.HasKey(e => e.Id); // Primary key

                entity.Property(e => e.Name)
                    .IsRequired(); // Not null

                entity.Property(e => e.IsDeleted)
                    .IsRequired(); // Not null

                entity.Property(e => e.MasterTypeId)
                    .IsRequired(); // Not null

                entity.Property(e => e.CreatedOn)
                    .IsRequired(); // Not null

                entity.Property(e => e.UpdatedOn)
                    .IsRequired(false); // Nullable
            });

            modelBuilder.Entity<EmployeeLeave>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Employee)
                      .WithMany()
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.LeaveType)
                      .WithMany()
                      .HasForeignKey(e => e.LeaveTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.AvailableLeave)
                      .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PendingLeave)
                      .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CreatedOn)
                      .IsRequired()
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false);
            });

            modelBuilder.Entity<LeaveRequest>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                       .IsRequired()
                       .ValueGeneratedOnAdd();

                entity.HasOne(e => e.Employee)
                      .WithMany()
                      .HasForeignKey(e => e.EmployeeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.LeaveType)
                      .WithMany()
                      .HasForeignKey(e => e.LeaveTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.LeaveDuration)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(true);

                entity.Property(e => e.EndDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.Note)
                      .HasMaxLength(500);

                entity.Property(e => e.Document)
                      .HasColumnType("nvarchar(max)");

                entity.Property(e => e.RequestStatus)
                      .IsRequired()
                      .HasMaxLength(30);

                entity.Property(e => e.CreatedOn)
                      .IsRequired()
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.UpdateOn)
                      .IsRequired(false);

            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ContactName)
                    .HasMaxLength(100);

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(100);

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(20);

                entity.Property(e => e.Address)
                    .HasMaxLength(255);

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true);

                entity.Property(e => e.CreatedOn)
                    .IsRequired();

                entity.Property(e => e.UpdatedOn)
                    .IsRequired(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Description)
                      .HasMaxLength(255);

                entity.Property(e => e.Price)
                      .HasColumnType("decimal(10, 2)")
                      .IsRequired();

                entity.Property(e => e.QuantityInStock)
                      .HasDefaultValue(0);

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true);

                entity.Property(e => e.CreatedOn)
                      .IsRequired();

                entity.HasOne(e => e.Brand)
                      .WithMany()
                      .HasForeignKey(e => e.BrandId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Supplier)
                      .WithMany()
                      .HasForeignKey(e => e.SupplierId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<CustomerRecord>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne<Customer>()
                      .WithMany() 
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne<Product>()
                      .WithMany() 
                      .HasForeignKey(e => e.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<Master>()
                      .WithMany() 
                      .HasForeignKey(e => e.TreatmentId)
                      .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne<Employee>()
                      .WithMany()
                      .HasForeignKey(e => e.AttendedEmployeeId)
                      .OnDelete(DeleteBehavior.SetNull); 

                // Configure the properties
                entity.Property(e => e.Strength)
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.Property(e => e.DevTime)
                      .IsRequired(false); 

                entity.Property(e => e.Remark)
                      .HasMaxLength(500)
                      .IsRequired(false);

                entity.Property(e => e.CreatedOn)
                      .IsRequired();

                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false); 

                entity.ToTable("CustomerRecords");
            });

            modelBuilder.Entity<InventoryHistory>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Product)
                    .WithMany()
                    .HasForeignKey(e => e.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.QuantityInStock)
                    .IsRequired()
                    .HasDefaultValue(0);  

                entity.Property(e => e.UpdatedQuantity)
                    .IsRequired();

                entity.Property(e => e.CreatedOn)
                    .IsRequired();

            });
        }

    }
}
