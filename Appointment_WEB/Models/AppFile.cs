using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Appointment_WEB.Models
{
    public class AppFile
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
    }

}
