using BW_Beverages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BW_Beverages.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            if (loginViewModel.UserName != null && loginViewModel.Password != null)
            {
                var user = await _userManager.FindByNameAsync(loginViewModel.UserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        if (string.IsNullOrEmpty(loginViewModel.ReturnUrl))
                            return RedirectToAction("Index", "Home");

                        return Redirect(loginViewModel.ReturnUrl);
                    }
                }
            }

            ModelState.AddModelError("", "Username/password not found");
            return View(loginViewModel);
        }

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if (registerViewModel.Password != null)
                {
                    var user = new IdentityUser { UserName = registerViewModel.UserName };
                    var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("LoggedIn");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("Password", "Password cannot be null.");
                }
            }
            return View(registerViewModel);
        }

        public IActionResult LoggedIn() => View();

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
