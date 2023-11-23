using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment_WEB.Models
{
    public class UserModel : IdentityUser
    {
        public byte[] UserProfileImage { get; set; }
    }
}
