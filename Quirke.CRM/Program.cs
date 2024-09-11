using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Quirke.CRM.DataContext;
using Quirke.CRM.Domain.Services;
using Quirke.CRM.Models;
using Quirke.CRM.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuration and Environment setup
var configuration = builder.Configuration;
var environment = builder.Environment;

// Logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net("log4net.config");
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

// Database context configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
);

// Identity configuration
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Razor Pages and MVC configuration with authorization policy
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

// Data protection configuration
//builder.Services.AddDataProtection()
//    .PersistKeysToDbContext<ApplicationDbContext>()
//    .SetApplicationName("QuirkeCRM");

// JSON serializer settings
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

// Suppressing implicit required attributes for non-nullable reference types
builder.Services.AddControllers(options =>
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
);

// Identity options configuration
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 6;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

// Cookie settings for authentication
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
});

// Routing configuration for lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Scoped services registration
//builder.Services.AddScoped<CheckUserLoggedInFilter>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IMasterService, MasterService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<ICommonService, CommonService>();

// MVC with filters
//builder.Services.AddControllersWithViews(options =>
//{
//    options.Filters.Add<CheckUserLoggedInFilter>();
//});

// Application build and middleware pipeline setup
var app = builder.Build();

// Error handling and HSTS setup for production
if (!environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTP and static file configurations
app.UseHttpsRedirection();
app.UseStaticFiles();

// Authentication and authorization middlewares
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Default route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Run the application
app.Run();
