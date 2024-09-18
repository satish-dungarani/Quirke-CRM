using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.DataContext;
using Quirke.CRM.Models;

namespace Quirke.CRM.Controllers
{
    public class BaseController : Controller
    {
        protected UserManager<ApplicationUser> userManager { get; }
        protected SignInManager<ApplicationUser> signInManager { get; }

        protected RoleManager<IdentityRole> roleManager { get; }
        protected ApplicationDbContext _context { get; set; }

        protected ApplicationUser CurrentUser => userManager.FindByNameAsync(User.Identity.Name).Result;

        public BaseController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager = null)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this._context = context;
        }

        public void AddModelError(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
        }
    }
}
