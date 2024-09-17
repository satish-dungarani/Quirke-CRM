using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quirke.CRM.Common;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain;
using Quirke.CRM.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Quirke.CRM.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Customers
        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.OrderByDescending(c=>c.Id).ToListAsync();
        }

        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }
        public async Task<CustomerModel> GetCustomerByIdModelAsync(int customerId)
        {
            var client =  await GetCustomerByIdAsync(customerId) ?? throw new KeyNotFoundException($"Client with ID {customerId} not found.");

            return new CustomerModel()
            {
                Id = client.Id,
                Firstname = client.Firstname,
                Lastname = client.Lastname,
                BirtDate = client.BirtDate,
                Gender = client.Gender,
                Mobile = client.Mobile,
                Email = client.Email,
                DispCreatedOn = client.CreatedOn.ToString("dd MMM yyyy")
            };
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


        #endregion

        #region Customers Compliance

        public async Task<IEnumerable<CustomerCompliance>> GetAllCustomerCompliancesAsync()
        {
            return await _context.CustomerCompliances.OrderByDescending(c => c.Id).ToListAsync();
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
                IsIdentityProvided = model.IsIdentityProvided,
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
                SignatureData = model.SignatureData,
                SalonSignatureData = model.SalonSignatureData,
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
                compliance.IsIdentityProvided = model.IsIdentityProvided;
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
                compliance.SignatureData = model.SignatureData;
                compliance.SalonSignatureData = model.SalonSignatureData;

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
                                 .OrderByDescending(c => c.Id)
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
                                 .AnyAsync(cc => cc.CustomerId == customer.Id
                                 && (cc.TestDate == null || cc.TestDate > DateTime.Now.AddMonths(-6)));
        }

        public async Task<bool> HasActiveComplianceAsync(int customerId)
        {
            DateTime sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);

            var isActive = await _context.CustomerCompliances.AnyAsync(c => c.CustomerId == customerId && c.TestDate >= sixMonthsAgo);

            return isActive;
        }

        public async Task<CustomerComplianceModel> GetCustomerComplianceModelByIdAsync(int id)
        {
            var compliance = await _context.CustomerCompliances.FindAsync(id);
            var customer = await _context.Customers.FindAsync(compliance?.Id);

            if (customer == null)
            {
                return new CustomerComplianceModel();
            }
            return new CustomerComplianceModel()
                {
                    Id = compliance.Id,
                    CustomerId = customer.Id,
                    Firstname = customer.Firstname,
                    Lastname = customer.Lastname,
                    BirthDate = customer.BirtDate,
                    DispBirthDate = customer.BirtDate.ToString("dd MMM yyyy"),
                    Mobile = customer.Mobile,
                    Status = compliance.Status,
                    IsIdentityProvided = compliance.IsIdentityProvided,
                    IsAllergicToColour = compliance.IsAllergicToColour,
                    AllergicColourDetails = compliance.AllergicColourDetails,
                    IsDamagedScalp = compliance.IsDamagedScalp,
                    ScalpDetails = compliance.ScalpDetails,
                    HasTattoo = compliance.HasTattoo,
                    TattooDetails = compliance.TattooDetails,
                    IsAllergicToProduct = compliance.IsAllergicToProduct,
                    AllergicProductDetails = compliance.AllergicProductDetails,
                    CanTakeService = compliance.CanTakeService,
                    IsAllergyTestDone = compliance.IsAllergyTestDone,
                    TestScheduleOn = compliance.TestScheduleOn,
                    DispTestScheduleOn = compliance.TestScheduleOn.HasValue ? compliance.TestScheduleOn?.ToString("dd MMM yyyy") : "",
                    DispTestDate = compliance.TestDate?.ToString("dd MMM yyyy"),
                    TestDate = compliance.TestDate,
                    ObservedBy = compliance.ObservedBy,
                    CreatedOn = compliance.CreatedOn,
                    DispCreatedOn = compliance.CreatedOn.ToString("dd MMM yyyy"),
                    UpdatedOn = compliance.UpdatedOn,
                    IsValid = compliance.TestDate == null || compliance.TestDate > DateTime.Now.AddMonths(-6),
                    SignatureData = compliance.SignatureData,
                    TestDueDate = compliance.TestDate?.AddMonths(6).ToString("dd MMM yyyy"),
                    SalonSignatureData = compliance.SalonSignatureData,
                };
            
        }
        public async Task<CustomerComplianceModel?> GetCustomerComplianceByMobile(string mobile)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Mobile == mobile);

            if (customer == null)
            {
                return new CustomerComplianceModel();
            }

            var compliance = await _context.CustomerCompliances
                                  .Where(cc => cc.CustomerId == customer.Id
                                  && (cc.TestDate == null || cc.TestDate > DateTime.Now.AddMonths(-6))).OrderByDescending(c => c.TestDate).FirstOrDefaultAsync();

            if (compliance == null)
            {
                return new CustomerComplianceModel()
                {
                    CustomerId = customer.Id,
                    Firstname = customer.Firstname,
                    Lastname = customer.Lastname,
                    BirthDate = customer.BirtDate,
                    Mobile = customer.Mobile
                };
            }
            else
            {
                return new CustomerComplianceModel()
                {
                    Id = compliance.Id,
                    CustomerId = customer.Id,
                    Firstname = customer.Firstname,
                    Lastname = customer.Lastname,
                    BirthDate = customer.BirtDate,
                    DispBirthDate = customer.BirtDate.ToString("dd MMM yyyy"),
                    Mobile = customer.Mobile,
                    Status = compliance.Status,
                    IsIdentityProvided = compliance.IsIdentityProvided,
                    IsAllergicToColour = compliance.IsAllergicToColour,
                    AllergicColourDetails = compliance.AllergicColourDetails,
                    IsDamagedScalp = compliance.IsDamagedScalp,
                    ScalpDetails = compliance.ScalpDetails,
                    HasTattoo = compliance.HasTattoo,
                    TattooDetails = compliance.TattooDetails,
                    IsAllergicToProduct = compliance.IsAllergicToProduct,
                    AllergicProductDetails = compliance.AllergicProductDetails,
                    CanTakeService = compliance.CanTakeService,
                    IsAllergyTestDone = compliance.IsAllergyTestDone,
                    TestScheduleOn = compliance.TestScheduleOn,
                    DispTestScheduleOn = compliance.TestScheduleOn.HasValue ? compliance.TestScheduleOn?.ToString("dd MMM yyyy") : "",
                    DispTestDate = compliance.TestDate?.ToString("dd MMM yyyy"),
                    TestDate = compliance.TestDate,
                    ObservedBy = compliance.ObservedBy,
                    CreatedOn = compliance.CreatedOn,
                    DispCreatedOn = compliance.CreatedOn.ToString("dd MMM yyyy"),
                    UpdatedOn = compliance.UpdatedOn,
                    IsValid = compliance.TestDate == null || compliance.TestDate > DateTime.Now.AddMonths(-6),
                    SignatureData = compliance.SignatureData,
                    TestDueDate = compliance.TestDate?.AddMonths(6).ToString("dd MMM yyyy"),
                    SalonSignatureData = compliance.SalonSignatureData,
                };
            }
        }
        #endregion

        #region Customer Records

        public async Task<IEnumerable<CustomerRecordModel>> GetAllCustomerRecordsByCustomerIdAsync(int customerId)
        {
            var customerRecords = await _context.CustomerRecords
                .Where(record => record.CustomerId == customerId)
                .ToListAsync(); // Pull the data from the database first

            var result = customerRecords
                .GroupJoin(
                    _context.Products.AsEnumerable(), // Join in-memory
                    record => record.ProductId,
                    product => product.Id,
                    (record, productGroup) => new { record, product = productGroup.FirstOrDefault() }
                )
                .GroupJoin(
                    _context.Masters.AsEnumerable(), // Join in-memory
                    record => record.record.TreatmentId,
                    treatment => treatment.Id,
                    (record, treatmentGroup) => new { record.record, record.product, treatment = treatmentGroup.FirstOrDefault() }
                )
                .GroupJoin(
                    _context.Employees.AsEnumerable(), // Join in-memory
                    record => record.record.AttendedEmployeeId,
                    employee => employee.Id,
                    (record, employeeGroup) => new { record.record, record.product, record.treatment, employee = employeeGroup.FirstOrDefault() }
                )
                .Select(record => new CustomerRecordModel
                {
                    Id = record.record.Id,
                    CustomerId = record.record.CustomerId,
                    ProductId = record.product?.Id,
                    TreatmentId = record.treatment?.Id,
                    ServiceDate = record.record.ServiceDate,
                    DispServiceDate = record.record.ServiceDate.ToString("dd MMM yyyy"),
                    Strength = record.record.Strength,
                    DevTime = record.record.DevTime,
                    Remark = record.record.Remark,
                    AttendedEmployeeId = record.record.AttendedEmployeeId,
                    CreatedOn = record.record.CreatedOn,
                    DispCreatedOn = record.record.CreatedOn.ToString("dd MMM yyyy"),
                    UpdatedOn = record.record.UpdatedOn,
                    ProductName = record.product?.Name ?? "",
                    TreatmentName = record.treatment?.Name ?? "",
                    AttendedEmployeeName = record.employee != null ? $"{record.employee.Firstname} {record.employee.Lastname}" : ""
                })
                .OrderByDescending(c => c.Id)
                .ToList();

            return result;
        }




        public async Task<CustomerRecordModel> GetCustomerRecordByIdAsync(int id)
        {
            var record = await _context.CustomerRecords.FindAsync(id);

            if (record == null)
            {
                throw new KeyNotFoundException($"Customer record with ID {id} not found.");
            }
            var model = new CustomerRecordModel()
            {
                Id = id,
                CustomerId = record.CustomerId,
                ProductId = record.ProductId,
                AttendedEmployeeId = record.AttendedEmployeeId,
                DevTime = record.DevTime,
                DispServiceDate = record.ServiceDate.ToString("dd MMM yyyy"),
                Strength = record.Strength,
                CreatedOn = record.CreatedOn,
                DispCreatedOn = record.CreatedOn.ToString("dd MMM yyyy"),
                Remark = record.Remark,
                TreatmentId = record.TreatmentId,
                UpdatedOn = record.UpdatedOn
            };

            return model;
        }

        public async Task CreateCustomerRecordAsync(CustomerRecordModel customerRecordModel)
        {
                var record = new CustomerRecord
                {
                    CustomerId = customerRecordModel.CustomerId,
                    ProductId = customerRecordModel.ProductId,
                    TreatmentId = customerRecordModel.TreatmentId,
                    ServiceDate = customerRecordModel.ServiceDate,
                    Strength = customerRecordModel.Strength,
                    DevTime = customerRecordModel.DevTime,
                    Remark = customerRecordModel.Remark,
                    AttendedEmployeeId = customerRecordModel.AttendedEmployeeId,
                    CreatedOn = DateTime.Now,
                };

                _context.CustomerRecords.Add(record);
                await _context.SaveChangesAsync();
         
        }

        public async Task UpdateCustomerRecordAsync(CustomerRecordModel customerRecordModel)
        {
            var record = await _context.CustomerRecords.FirstOrDefaultAsync(r => r.Id == customerRecordModel.Id);

            if (record == null)
            {
                throw new KeyNotFoundException($"Customer record with ID {customerRecordModel.Id} not found.");
            }

            record.CustomerId = customerRecordModel.CustomerId;
            record.ProductId = customerRecordModel.ProductId;
            record.TreatmentId = customerRecordModel.TreatmentId;
            record.ServiceDate = customerRecordModel.ServiceDate;
            record.Strength = customerRecordModel.Strength;
            record.DevTime = customerRecordModel.DevTime;
            record.Remark = customerRecordModel.Remark;
            record.AttendedEmployeeId = customerRecordModel.AttendedEmployeeId;
            record.UpdatedOn = DateTime.Now;

            _context.CustomerRecords.Update(record);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCustomerRecordAsync(int id)
        {
            var record = await _context.CustomerRecords.FirstOrDefaultAsync(r => r.Id == id);
            if (record == null)
            {
                return false;
            }
            _context.CustomerRecords.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<SelectListItem>> GetProductListAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Brand)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Name} {"|"} {s.Brand.Name}"
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetTreatmentListAsync()
        {
            return await _context.Masters
                .Where(m => m.MasterTypeId == (int)MasterType.Treatment)
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetEmployeeListAsync()
        {
            return await _context.Employees
                .Where(e => !e.IsDeleted)
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = $"{e.Firstname} {e.Lastname}"
                })
                .ToListAsync();
        }
       
        #endregion
    }
}
