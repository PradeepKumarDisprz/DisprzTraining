namespace DisprzTraining.Model
{
    //response for creation
    public class NewAppointmentId
    {
        public Guid appointmentId { get; set; }
    }

    public class ErrorDetails
    {
        public string language { get; set; }="en";
        public string errorMessage { get; set; }=string.Empty;
        public int errorCode { get; set; }
    }

    public class AllAppointments
    {
        public int count { get; set; }
        public bool isTruncated { get; set; }
        public List<Appointment>? appointments { get; set; }


    }
}  