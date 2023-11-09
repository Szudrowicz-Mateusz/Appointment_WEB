using Appointment_WEB.Models;
using Appointment_WEB.Services.Interfaces;
using Appointment_WEB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Appointment_WEB.Email.Interfaces;

namespace Appointment_WEB.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<UserModel> _userManager;

        public AppointmentController(IAppointmentService appointmentService,IEmailSender emailSender, UserManager<UserModel> userManager)
        {
            _appointmentService = appointmentService;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public IActionResult Show()
        {
            var appointments = _appointmentService.GetAll();

            return View(appointments);
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AppointmentModel model)
        {
            if (ModelState.IsValid)
            {
                List<AppointmentModel> ap = _appointmentService.GetAll();

                // Check for overlapping appointments
                bool isOverlapping = ap.Any(a =>
                    a.day == model.day &&
                    model.startTime < a.endTime && model.endTime > a.startTime);

                if (isOverlapping)
                {
                    ViewData["WarningMessage"] = "The new appointment overlaps with an existing appointment.";
                    return View(model);
                }

                // Get the currently logged-in user's email
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    string userEmail = user.Email;
                    string subject = "You have created a new appointment";
                    string message = $"Hello, \nyou created a new appointment on next {model.day}, called {model.title} starting at {model.startTime} and ending on {model.endTime}." +
                        $"\nThanks for you time and wish you great meeting ";

                     await _emailSender.SendEmailAsync(userEmail, subject, message);
                    
                    Console.WriteLine(subject);
                    
                }

                _appointmentService.Save(model);

                return RedirectToAction("Show");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id) 
        {
            _appointmentService.Delete(id);
            return RedirectToAction("Show");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id, string title, string description) 
        {
            _appointmentService.Edit(id, title, description);
            return RedirectToAction("Show");
        }
    }
}
    


