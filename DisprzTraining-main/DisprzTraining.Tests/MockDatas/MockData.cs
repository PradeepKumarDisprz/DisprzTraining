using DisprzTraining.Model;

namespace DisprzTraining.Tests.MockDatas
{
    public static class MockData
    {

        public static AppointmentDTO MockAppointment = new AppointmentDTO()
        {
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        };

        public static Appointment MockAppointmentByID = new Appointment()
        {
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        
        };

        public static List<Appointment> MockAppointmentList = new List<Appointment>()
        {
         new Appointment{
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        },
         new Appointment{
            appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
            appointmentStartTime = DateTime.Now,
            appointmentEndTime = DateTime.Now.AddHours(1),
            appointmentTitle = "worldCup Discussion",
            appointmentDescription = "will messi win the WC"
        }
        };
}
}