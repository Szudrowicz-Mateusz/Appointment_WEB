using Appointment_WEB.Models;
using Appointment_WEB.Services.Interfaces;
using Appointment_WEB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Appointment_WEB.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
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
        public IActionResult Add(AppointmentModel model)
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

                _appointmentService.Save(model);

                return RedirectToAction("Show");
            }

            return View(model); // If the model is not valid, return to the AddAppointments view with validation errors.
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
    


