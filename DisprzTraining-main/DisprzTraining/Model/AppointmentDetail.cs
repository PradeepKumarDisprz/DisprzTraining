using System.ComponentModel.DataAnnotations;

namespace DisprzTraining.Model
{
    public class AppointmentDetail
    {
        [Required]   
        public DateTime appointmentStartTime { get; set; }
        [Required]
        public DateTime appointmentEndTime { get; set; }
        [Required]
        public string appointmentTitle { get; set; }=string.Empty;
        public string appointmentDescription { get; set; }=string.Empty;  
    }
}