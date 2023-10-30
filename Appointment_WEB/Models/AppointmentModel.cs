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

    public class RangeTimeOfAppointments : ValidationAttribute 
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var appointment = (AppointmentModel)validationContext.ObjectInstance;

            var earlier = new TimeSpan(7, 59, 0);
            var later = new TimeSpan(20, 1, 0);

            if(appointment.startTime<= earlier || appointment.startTime >= later)
            {
                return new ValidationResult("Start time must be between 8AM to 8PM");
            }
            else if(appointment.endTime <= earlier || appointment.endTime >= later)
            {
                return new ValidationResult("End time must be between 8AM to 8PM");
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
        [MaxLength(20)] // I don'y use ErrorMessage couse my form didn't allow you to make more then MaxLength
        public String title { get; set; }
        [Required]
        [MaxLength(30)] 
        public string description { get; set; }
        [Required]
        public Day day { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [StartTimeEarlierThanEndTime]
        public TimeSpan startTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [RangeTimeOfAppointments]
        public TimeSpan endTime { get; set; }
    }
}
