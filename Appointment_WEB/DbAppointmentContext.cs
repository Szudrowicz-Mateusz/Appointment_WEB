using Appointment_WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace Appointment_WEB
{
    public class DbAppointmentContext : DbContext
    {
        public DbSet<AppointmentModel> Appointments {get; set;}

        public DbAppointmentContext(DbContextOptions options) : base(options) { }
    }
}
