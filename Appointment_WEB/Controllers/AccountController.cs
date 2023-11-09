using Appointment_WEB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Appointment_WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;

        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login userLoginData)
        {
            if(!ModelState.IsValid)
            {
                return View(userLoginData);
            }

            await _signInManager.PasswordSignInAsync(userLoginData.Name, userLoginData.Password,false,false);

            return RedirectToAction("Show", "Appointment");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register userRegisterData) 
        {
            if(!ModelState.IsValid)
            {
                return View(userRegisterData);
            }

            await _userManager.CreateAsync(new UserModel
            {
                Email = userRegisterData.Email,
                UserName = userRegisterData.Name,
                PhoneNumber = userRegisterData.Phone
            }, userRegisterData.Password);

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> ShowFullProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                return View(user);
            }
            return RedirectToAction("Login"); // Handle the case where the user is not found or not logged in.
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel userNewPassword)
        {
            if (!ModelState.IsValid || userNewPassword.NewPassword != userNewPassword.RepeatNewPassword)
            {
                ModelState.AddModelError(string.Empty, "Failed to change password. Please check your current password and try again.");
                return View(userNewPassword);
            }

            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // Change the user's password
                var result = await _userManager.ChangePasswordAsync(user, userNewPassword.OldPassword, userNewPassword.NewPassword);

                if (result.Succeeded)
                {
                    // Redirect to logout or another action
                    return RedirectToAction("LogOut");
                }
                else
                {
                    // Handle password change failure
                    ModelState.AddModelError(string.Empty, "Failed to change password.");
                    return View(userNewPassword);
                }
            }

            // Redirect to login or another action
            return RedirectToAction("Login");
        }

    }
}
