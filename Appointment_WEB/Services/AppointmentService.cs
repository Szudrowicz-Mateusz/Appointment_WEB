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
            var appointemnt = _context.Appointments.Find(id);
            _context.Appointments.Remove(appointemnt);

            if(_context.SaveChanges()>0)
            {
                System.Console.WriteLine("Success");
            }

            return id;
        }

        public int Edit(int id)
        {
            // To be done
            return 0;
        }
    }
}
