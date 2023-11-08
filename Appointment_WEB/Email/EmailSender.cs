using Appointment_WEB.Email.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Appointment_WEB.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email,string subject, string description) 
        {
            var emailSenderAddress = "appointment_sender@outlook.com";
            var emailSenderPassword = "poiuytrewq1234";

            var client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(emailSenderAddress, emailSenderPassword)
            };

            return client.SendMailAsync(new MailMessage(from: emailSenderAddress,
                                                        to: email,
                                                        subject,
                                                        description));
        }
    }
}
