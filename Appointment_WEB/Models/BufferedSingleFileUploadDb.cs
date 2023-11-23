using System.ComponentModel.DataAnnotations;

namespace Appointment_WEB.Models
{
    public class BufferedSingleFileUploadDb
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
    }


}
