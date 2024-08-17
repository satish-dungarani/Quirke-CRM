using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quirke.CRM.Common;
using Quirke.CRM.DataContext;
using Quirke.CRM.Models;

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
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByEmailAsync(userLoginModel.Email);

                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, userLoginModel.Password, userLoginModel.RememberMe, false);

                        if (result.Succeeded)
                        {
                            TempData["SuccessMessage"] = "Successfully login!";
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Invalid password!";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "No user found with this email address!";
                    }
                }

                return View(userLoginModel);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(AccountController)} - {nameof(Login)} - ERROR - {ex}");
                return View("CustomError");
            }

        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserRegisterModel userRegisterModel)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    if (_userManager.Users.Any(u => u.Email == userRegisterModel.Email))
                    {
                        TempData["WarningMessage"] = "This email address is already registered.";
                        return View(userRegisterModel);
                    }

                    if (_userManager.Users.Any(u => u.PhoneNumber == userRegisterModel.PhoneNumber))
                    {
                        TempData["WarningMessage"] = "This phone number is already registered.";
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
                        TempData["SuccessMessage"] = "Registered successfully!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "An error occurred during registration!";
                    }
                }
                return View(userRegisterModel);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error($"{nameof(AccountController)} - {nameof(SignUp)} - ERROR - {ex}");
                return View("CustomError");
            }
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

    }
}
