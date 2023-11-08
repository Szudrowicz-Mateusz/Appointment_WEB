namespace Appointment_WEB.Email.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email,string subject,string description);
    }
}
