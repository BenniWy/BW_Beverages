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
            Console.WriteLine("+++++UserManager: " +_userManager+ "\n");
            _signInManager = signInManager;
            Console.WriteLine("+++++SignInManager: " +_signInManager+ "\n");
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

            ModelState.AddModelError("", "Username/password not found");
            return View(loginViewModel);
        }

        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            Console.WriteLine("\n+++++ ModelState valid?: " + ModelState.IsValid +" +++++\n");
            Console.WriteLine("\n+++++ ModelState: " + ModelState+" +++++\n");
            Console.WriteLine("\n+++++ registerViewModel: " +registerViewModel+" +++++\n");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = registerViewModel.UserName };
                Console.WriteLine("\n+++++ user: " +user+" +++++\n");
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                Console.WriteLine("\n+++++ result: " +result+" +++++\n");
                Console.WriteLine("\n+++++ result.Succeeded?: " +result.Succeeded+" +++++\n");

                if (result.Succeeded)
                {
                    return RedirectToAction("LoggedIn");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
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
