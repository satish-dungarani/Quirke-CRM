using Quirke.CRM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using Quirke.CRM.DataContext;

namespace Quirke.CRM.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
            : base(userManager, signInManager, context, roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLoginModel userLoginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginModel.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginModel.Password, userLoginModel.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        SetError("Invalid password", userLoginModel);
                    }
                }
                else
                {
                    SetError("No user found with this email address.", userLoginModel);
                }
            }

            return View(userLoginModel);
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserRegisterModel userRegisterModel)
        {
            if (ModelState.IsValid)
            {
                if (_userManager.Users.Any(u => u.Email == userRegisterModel.Email))
                {
                    SetError("This email address is already registered.", userRegisterModel);
                    return View(userRegisterModel);
                }

                if (_userManager.Users.Any(u => u.PhoneNumber == userRegisterModel.PhoneNumber))
                {
                    SetError("This phone number is already registered.", userRegisterModel);
                    return View(userRegisterModel);
                }

                var user = new ApplicationUser
                {
                    UserName = userRegisterModel.UserName,
                    Email = userRegisterModel.Email,
                    PhoneNumber = userRegisterModel.PhoneNumber,
                    FirstName = userRegisterModel.FirstName,
                    LastName = userRegisterModel.LastName
                };

                var result = await _userManager.CreateAsync(user, userRegisterModel.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    SetError("An error occurred during registration.", userRegisterModel);
                    AddModelError(result);
                }
            }

            return View(userRegisterModel);
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult ForgetPassword()
        {
            ViewBag.Success = false;
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(UserLoginModel userLoginModel)
        {
            // Add reset password logic here
            throw new NotImplementedException();
        }

        private void SetError(string message, object model)
        {
            ModelState.AddModelError("", message);
            ViewBag.Message = message;
        }
    }
}
