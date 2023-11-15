using Appointment_WEB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Appointment_WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
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

        private byte[] ConvertToByteArray(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register userRegisterData, IFormFile profileImage = null)
        {
            if (!ModelState.IsValid)
            {
                return View(userRegisterData);
            }

            var user = new UserModel
            {
                Email = userRegisterData.Email,
                UserName = userRegisterData.Name,
                PhoneNumber = userRegisterData.Phone,
                // Set the user's profile image or use a default image
                UserProfileImage = profileImage != null ? ConvertToByteArray(profileImage) : GetDefaultProfileImage()
            };

            await _userManager.CreateAsync(user, userRegisterData.Password);

            return RedirectToAction("Login");
        }

        private byte[] GetDefaultProfileImage()
        {
            // Load the default profile image from the project
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "PersonIcon.png");

            // Convert the image to a byte array
            return System.IO.File.ReadAllBytes(imagePath);
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

        [HttpGet]
        public IActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(BufferedSingleFileUploadDb fileModel)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    // Convert IFormFile to byte array
                    user.UserProfileImage = ConvertToByteArray(fileModel.FormFile);

                    // Update the user in the database
                    await _userManager.UpdateAsync(user);

                    // Redirect to profile or another action
                    return RedirectToAction("ShowFullProfile");
                }
            }

            // If something goes wrong, return to the upload form
            return View(fileModel);
        }

    }
}
