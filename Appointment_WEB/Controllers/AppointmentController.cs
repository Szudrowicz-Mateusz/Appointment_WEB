using Appointment_WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace Appointment_WEB.Controllers
{
    public class AppointmentController : Controller
    {
        private List<AppointmentModel> appointments = new List<AppointmentModel>();

        public IActionResult Show()
        {

            // Dummy data
            var dummyAppointments = new List<AppointmentModel>
              {
                new AppointmentModel(1, "Title1", "Desc1", Day.Thursday, new TimeSpan(10, 0, 0), new TimeSpan(12, 0, 0)),
                new AppointmentModel(2, "Title2", "Desc2", Day.Monday, new TimeSpan(8, 0, 0), new TimeSpan(15, 0, 0))
            };

            return View(dummyAppointments);
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
                
                appointments.Add(model);

                
                return RedirectToAction("Show");
            }
            return View(model);
        }
    }

}
