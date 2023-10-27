using System.ComponentModel.DataAnnotations;

namespace Appointment_WEB.Models
{
    public enum Day
    {
        Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday
    }

    public class StartTimeEarlierThanEndTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var appointment = (AppointmentModel)validationContext.ObjectInstance;

            if (appointment.startTime >= appointment.endTime)
            {
                return new ValidationResult("Start time must be earlier than end time.");
            }

            return ValidationResult.Success;
        }
    }


    public class AppointmentModel
    {
        public AppointmentModel() { }
        public AppointmentModel(int id, String title, String description, Day day,TimeSpan startTime,TimeSpan endTime) 
        {
            this.id = id; this.title = title; this.description = description; this.day = day; this.startTime = startTime; this.endTime = endTime;
        }

        [Key]
        public int id { get; set; }
        [Required]
        public String title { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public Day day { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [StartTimeEarlierThanEndTime]
        public TimeSpan startTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan endTime { get; set; }
    }
}
