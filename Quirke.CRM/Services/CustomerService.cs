﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain;
using Quirke.CRM.Models;

namespace Quirke.CRM.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Customer methods
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        // CustomerCompliance methods
        public async Task<IEnumerable<CustomerCompliance>> GetAllCustomerCompliancesAsync()
        {
            return await _context.CustomerCompliances.ToListAsync();
        }

        public async Task<CustomerCompliance> GetCustomerComplianceByIdAsync(int customerComplianceId)
        {
            return await _context.CustomerCompliances.FindAsync(customerComplianceId);
        }

        public async Task<int> CreateCustomerComplianceAsync(CustomerComplianceModel model)
        {
            var compliance = new CustomerCompliance
            {
                CustomerId = model.CustomerId,
                IsAllergicToColour = model.IsAllergicToColour,
                AllergicColourDetails = model.AllergicColourDetails,
                IsDamagedScalp = model.IsDamagedScalp,
                ScalpDetails = model.ScalpDetails,
                HasTattoo = model.HasTattoo,
                TattooDetails = model.TattooDetails,
                IsAllergicToProduct = model.IsAllergicToProduct,
                AllergicProductDetails = model.AllergicProductDetails,
                TestScheduleOn = model.TestScheduleOn,
                TestDate = model.TestDate,
                Status = model.Status,
                ObservedBy = model.ObservedBy,
                CanTakeService = model.CanTakeService,
                IsAllergyTestDone = model.IsAllergyTestDone,
                CreatedOn = DateTime.UtcNow,
            };

            _context.CustomerCompliances.Add(compliance);
            await _context.SaveChangesAsync();
            return compliance.Id;
        }

        public async Task UpdateCustomerComplianceAsync(CustomerComplianceModel model)
        {
            var compliance = await _context.CustomerCompliances.FindAsync(model.Id);
            if (compliance != null)
            {
                compliance.IsAllergicToColour = model.IsAllergicToColour;
                compliance.AllergicColourDetails = model.AllergicColourDetails;
                compliance.IsDamagedScalp = model.IsDamagedScalp;
                compliance.ScalpDetails = model.ScalpDetails;
                compliance.HasTattoo = model.HasTattoo;
                compliance.TattooDetails = model.TattooDetails;
                compliance.IsAllergicToProduct = model.IsAllergicToProduct;
                compliance.AllergicProductDetails = model.AllergicProductDetails;
                compliance.TestScheduleOn = model.TestScheduleOn;
                compliance.TestDate = model.TestDate;
                compliance.Status = model.Status;
                compliance.ObservedBy = model.ObservedBy;
                compliance.CanTakeService = model.CanTakeService;
                compliance.IsAllergyTestDone = model.IsAllergyTestDone;

                _context.CustomerCompliances.Update(compliance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCustomerComplianceAsync(int customerComplianceId)
        {
            var customerCompliance = await _context.CustomerCompliances.FindAsync(customerComplianceId);
            if (customerCompliance != null)
            {
                _context.CustomerCompliances.Remove(customerCompliance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CustomerCompliance>> GetCustomerComplianceByCustomerIdAsync(int customerId)
        {
            return await _context.CustomerCompliances
                                 .Where(cc => cc.CustomerId == customerId)
                                 .ToListAsync();
        }

        public async Task<bool> IsAnyCustomerRegisteredByMobileNumberWithActiveComplianceAsync(string mobileNumber)
        {
            var customer = await _context.Customers
                                         .FirstOrDefaultAsync(c => c.Mobile == mobileNumber);
            if (customer == null)
            {
                return false;
            }

            return await _context.CustomerCompliances
                                 .AnyAsync(cc => cc.CustomerId == customer.Id && cc.Status.ToLower() == "active"); 
        }

        public async Task<bool> HasActiveComplianceAsync(int customerId)
        {
            DateTime sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);

            var isActive = await _context.CustomerCompliances.AnyAsync(c => c.CustomerId == customerId && c.TestDate >= sixMonthsAgo);

            return isActive;
        }
    }
}
