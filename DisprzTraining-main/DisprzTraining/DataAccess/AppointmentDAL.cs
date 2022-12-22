using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {
        public static List<Appointment> _userAppointments = new List<Appointment>();


        public Task<List<Appointment>> GetAppointments()
        {
            return Task.FromResult(_userAppointments);
        }
        
        //add new appointment and update existing appointment
        public void AddAppointments(List<Appointment> appointment)
        {
            _userAppointments=appointment;
        }

    }
}




















