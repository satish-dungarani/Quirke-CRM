using Microsoft.EntityFrameworkCore;
using Quirke.CRM.DataContext;
using Quirke.CRM.Models;
using Quirke.CRM.Services;

namespace Quirke.CRM.Domain.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _context;

        public SupplierService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<SupplierModel> CreateSupplierAsync(SupplierModel model)
        {
            var supplier = new Supplier
            {
                Name = model.Name,
                ContactName = model.ContactName,
                ContactEmail = model.ContactEmail,
                ContactPhone = model.ContactPhone,
                Address = model.Address,
                IsActive = model.IsActive,
                CreatedOn = DateTime.UtcNow,
            };

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            model.Id = supplier.Id;
            return model;
        }

        public async Task<SupplierModel> GetSupplierByIdAsync(int id)
        {
            var supplier = await _context.Suppliers
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (supplier == null)
            {
                return null;
            }

            return new SupplierModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                ContactName = supplier.ContactName,
                ContactEmail = supplier.ContactEmail,
                ContactPhone = supplier.ContactPhone,
                Address = supplier.Address,
                IsActive = supplier.IsActive,
                CreatedOn = supplier.CreatedOn,
                UpdatedOn = supplier.UpdatedOn
            };
        }

        public async Task<IEnumerable<SupplierModel>> GetAllSuppliersAsync()
        {
            
            try
            {
                var suppliers = await _context.Suppliers.ToListAsync();
                return suppliers.Select(s => new SupplierModel
                {
                    Id = s.Id,
                    Name = s.Name ?? string.Empty,
                    ContactName = s.ContactName ?? string.Empty,
                    ContactEmail = s.ContactEmail ?? string.Empty,
                    ContactPhone = s.ContactPhone ?? string.Empty,
                    Address = s.Address ?? string.Empty,
                    IsActive = s.IsActive,
                    CreatedOn = s.CreatedOn,
                    UpdatedOn = s.UpdatedOn
                });
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public async Task<SupplierModel> UpdateSupplierAsync(SupplierModel model)
        {
            var existingSupplier = await _context.Suppliers.FindAsync(model.Id) ?? throw new KeyNotFoundException($"Supplier with ID {model.Id} not found.");
            existingSupplier.Name = model.Name;
            existingSupplier.ContactName = model.ContactName;
            existingSupplier.ContactEmail = model.ContactEmail;
            existingSupplier.ContactPhone = model.ContactPhone;
            existingSupplier.Address = model.Address;
            existingSupplier.IsActive = model.IsActive;
            existingSupplier.UpdatedOn = model.UpdatedOn;

            await _context.SaveChangesAsync();

            return new SupplierModel
            {
                Id = existingSupplier.Id,
                Name = existingSupplier.Name,
                ContactName = existingSupplier.ContactName,
                ContactEmail = existingSupplier.ContactEmail,
                ContactPhone = existingSupplier.ContactPhone,
                Address = existingSupplier.Address,
                IsActive = existingSupplier.IsActive,
                CreatedOn = existingSupplier.CreatedOn,
                UpdatedOn = existingSupplier.UpdatedOn
            };
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return false;
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
