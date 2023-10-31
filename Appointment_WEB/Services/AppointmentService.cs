using Appointment_WEB.Models;
using Appointment_WEB.Services.Interfaces;
using System.Net.Http.Headers;

namespace Appointment_WEB.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly DbAppointmentContext _context;

        public AppointmentService(DbAppointmentContext context)
        {
            _context = context;
        }

        public int Save(AppointmentModel model)
        {
            _context.Add(model);

            if(_context.SaveChanges()>0) {
                System.Console.WriteLine("Success");            
            }

            return model.id;
        }

        public List<AppointmentModel> GetAll()
        {
            var appointments = _context.Appointments.ToList();

            return appointments;
        }

        public int Delete(int id)
        {
            var appointment = _context.Appointments.Find(id);
            _context.Appointments.Remove(appointment);

            if(_context.SaveChanges()>0)
            {
                System.Console.WriteLine("Success");
            }
            else
            {
                System.Console.WriteLine("Failure");
            }

            return id;
        }

        public int Edit(int id, string title, string description)
        {
            var appointment = _context.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.title = title;
                appointment.description = description;
                _context.SaveChanges();
                return id;
            }
            return -1; // Return a value to indicate that the update was unsuccessful
        }

    }
}
