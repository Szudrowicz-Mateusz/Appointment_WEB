using Appointment_WEB.Models;
using Appointment_WEB.Services.Interfaces;
using Appointment_WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace Appointment_WEB.Controllers
{
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
                _appointmentService.Save(model); 

                return RedirectToAction("Show");
            }
            return View(model);
        }
    }

}
