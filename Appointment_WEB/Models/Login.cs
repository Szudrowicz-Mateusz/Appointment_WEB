﻿using System.ComponentModel.DataAnnotations;

namespace Appointment_WEB.Models
{
    public class Login
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
