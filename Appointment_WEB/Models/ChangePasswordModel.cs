using System.ComponentModel.DataAnnotations;

namespace Appointment_WEB.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string RepeatNewPassword { get; set; }
    }
}
