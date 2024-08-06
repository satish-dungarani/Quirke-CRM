using Quirke.CRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quirke.CRM.Domain;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring Customer entity
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

            // Configuring CustomerCompliance entity
            modelBuilder.Entity<CustomerCompliance>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerId)
                      .IsRequired();
                entity.HasOne<Customer>()
                      .WithMany()
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);

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
                      .HasMaxLength(20)
                      .IsRequired(false);
                entity.Property(e => e.TestDate)
                      .IsRequired(false)
                      .HasColumnType("datetime");
                entity.Property(e => e.ObservedBy)
                      .IsRequired(false);
                entity.Property(e => e.CreatedOn)
                      .IsRequired()
                      .HasColumnType("datetime");
                entity.Property(e => e.UpdatedOn)
                      .IsRequired(false)
                      .HasColumnType("datetime");
            });
        }
    }
}
