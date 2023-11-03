using System.ComponentModel.DataAnnotations;

namespace Appointment_WEB.Models
{
	public class Register
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[Phone]
		public string Phone { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
