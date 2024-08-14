using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quirke.CRM.Common;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain;
using Quirke.CRM.Models;

namespace Quirke.CRM.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductModel> CreateProductAsync(ProductModel model)
        {
            var product = new Product
            {
                BrandId = model.BrandId,
                SupplierId = model.SupplierId,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                QuantityInStock = model.QuantityInStock,
                IsActive = model.IsActive,
                CreatedOn = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            model.Id = product.Id;

            if (product.Id > 0)
            {
                var history = new InventoryHistory()
                {
                    ProductId = product.Id,
                    QuantityInStock = model.QuantityInStock,
                    UpdatedQuantity = model.QuantityInStock,
                    CreatedOn = DateTime.UtcNow
                };

                _context.InventoryHistories.Add(history);
                await _context.SaveChangesAsync();
            }
            return model;
        }

        public async Task<ProductModel> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return null;
            }

            return new ProductModel
            {
                Id = product.Id,
                BrandId = product.BrandId,
                SupplierId = product.SupplierId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                IsActive = product.IsActive,
                CreatedOn = product.CreatedOn,
                UpdatedOn = product.UpdatedOn,
                BrandName = product.Brand?.Name,
                SupplierName = product.Supplier?.Name
            };
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Supplier)
                .Include(p => p.Brand)
                .ToListAsync();

            return products.Select(product => new ProductModel
            {
                Id = product.Id,
                BrandId = product.BrandId,
                SupplierId = product.SupplierId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                QuantityInStock = product.QuantityInStock,
                IsActive = product.IsActive,
                CreatedOn = product.CreatedOn,
                UpdatedOn = product.UpdatedOn,
                BrandName = product.Brand?.Name,
                SupplierName = product.Supplier?.Name
            });
        }

        public async Task<ProductModel> UpdateProductAsync(ProductModel model)
        {
            var product = await _context.Products.FindAsync(model.Id);
            if (product == null)
            {
                return null;
            }

            var history = new InventoryHistory()
            {
                ProductId = product.Id,
                QuantityInStock = product.QuantityInStock,
                UpdatedQuantity = model.QuantityInStock,
                CreatedOn = DateTime.UtcNow
            };

            _context.InventoryHistories.Add(history);

            product.BrandId = model.BrandId;
            product.SupplierId = model.SupplierId;
            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.QuantityInStock = model.QuantityInStock;
            product.IsActive = model.IsActive;
            product.UpdatedOn = DateTime.UtcNow;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();



            return model;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SelectListItem>> GetSupplierListAsync()
        {
            return await _context.Suppliers
                .Where(s => s.IsActive)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetBrandListAsync()
        {
            return await _context.Masters
                .Where(m => m.MasterTypeId == (int)MasterType.Brand)
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<InventoryHistoryModel>> GetAllInventoryHistoryByProductsAsync(int productId)
        {
            return await _context.InventoryHistories
                .Include(p => p.Product)
                .Where(p => p.ProductId == productId)
                .OrderByDescending(i => i.CreatedOn)
                .Select(i => new InventoryHistoryModel()
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    QuantityInStock = i.QuantityInStock,
                    UpdatedQuantity = i.UpdatedQuantity,
                    ProductName = i.Product.Name,
                    CreatedOn = i.CreatedOn,
                    Product = new ProductModel()
                    {
                        Id = i.Product.Id,
                        Name = i.Product.Name,
                    }
                }).ToListAsync();
        }

        public async Task<IEnumerable<InventoryHistoryModel>> GetLatestInventoryAsync()
        {
            var latestRecords = await _context.InventoryHistories
                .Include(p => p.Product)
                .GroupBy(i => i.ProductId)
                .Select(g => g.OrderByDescending(i => i.CreatedOn).FirstOrDefault())
                .ToListAsync();

            return latestRecords.Select(i => new InventoryHistoryModel()
            {
                Id = i.Id,
                ProductId = i.ProductId,
                QuantityInStock = i.QuantityInStock,
                UpdatedQuantity = i.UpdatedQuantity,
                ProductName = i.Product.Name,
                CreatedOn = i.CreatedOn,
                Product = new ProductModel()
                {
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                }
            });
        }
        public async Task<IEnumerable<InventoryHistoryModel>> GetAllInventoryHistoryAsync()
        {
            return await _context.InventoryHistories
                .Include(p => p.Product)
                .OrderByDescending(i => i.CreatedOn)
                .Select(i => new InventoryHistoryModel()
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    QuantityInStock = i.QuantityInStock,
                    UpdatedQuantity = i.UpdatedQuantity,
                    ProductName = i.Product.Name,
                    CreatedOn = i.CreatedOn,
                    Product = new ProductModel()
                    {
                        Id = i.Product.Id,
                        Name = i.Product.Name,
                    }
                }).ToListAsync();
        }
    }
}
