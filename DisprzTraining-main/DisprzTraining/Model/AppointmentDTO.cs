using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DisprzTraining.Model
{
    public class AppointmentDTO
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