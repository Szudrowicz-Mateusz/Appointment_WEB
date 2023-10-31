using Appointment_WEB.Models;

namespace Appointment_WEB.Services.Interfaces
{
    public interface IAppointmentService
    {
        int Save(AppointmentModel model);
        List<AppointmentModel> GetAll();
        int Edit(int id, string title, string description);
        int Delete(int id);
    }
}
