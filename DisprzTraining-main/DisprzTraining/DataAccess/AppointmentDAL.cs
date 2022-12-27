using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {
        public static List<Appointment> _userAppointments = new List<Appointment>()
        {
            new Appointment()
            {
                appointmentId=Guid.NewGuid(),
                appointmentStartTime=DateTime.Now,
                appointmentEndTime=DateTime.Now.AddHours(1),
                appointmentTitle="standup",
                appointmentDescription="meet is going to commence"
            }
        };




        public async Task<List<Appointment>> GetAppointments()
        {
            return await Task.FromResult(_userAppointments);
        }
        
        //add new appointment and update existing appointment
        public Task AddAppointments(List<Appointment> appointment)
        {
            _userAppointments=appointment;
            return Task.CompletedTask;
        }

    }
}




















