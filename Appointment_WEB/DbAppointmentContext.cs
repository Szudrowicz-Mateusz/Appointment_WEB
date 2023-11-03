using Appointment_WEB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Appointment_WEB
{
    public class DbAppointmentContext : IdentityDbContext<UserModel>
    {
        public DbSet<AppointmentModel> Appointments {get; set;}

        public DbAppointmentContext(DbContextOptions options) : base(options) { }
    }
}
