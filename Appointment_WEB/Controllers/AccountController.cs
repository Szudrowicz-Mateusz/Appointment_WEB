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
                    return RedirectToAction("LogOut");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to change password.");
                    return View(userNewPassword);
                }
            }

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
            // First four if is checking for wrong formats etc.

            if (!ModelState.IsValid)
                return View(fileModel);

            if (fileModel == null)
            {
                ModelState.AddModelError("File", "Please select a file.");
                return View(fileModel);
            }

            // Check if the file has an allowed extension
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(fileModel.FormFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("File", "Only .jpg, .jpeg, and .png file types are allowed.");
                return View(fileModel);
            }

            // Check if the file size is within the allowed limit (3MB)
            if (fileModel.FormFile.Length > 3145728)// 3MB in bytes
            {
                ModelState.AddModelError("File", "The file size exceeds the allowed limit (3MB).");
                return View(fileModel);
            }


            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                user.UserProfileImage = ConvertToByteArray(fileModel.FormFile);

                await _userManager.UpdateAsync(user);

                return RedirectToAction("ShowFullProfile");
            }


            return View(fileModel);
        }

    }
}
