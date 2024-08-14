using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Quirke.CRM.Models;

namespace Quirke.CRM.Filters
{
    public class CheckUserLoggedInFilter : IAsyncActionFilter
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public CheckUserLoggedInFilter(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //if (!_signInManager.IsSignedIn(context.HttpContext.User))
            //{
            //    context.Result = new RedirectToActionResult("Login", "Account", null);
            //    return;
            //}

            await next();
        }
    }
}
