using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        List<Appointment> GetAppointmentByDate(DateTime date);
        bool DeleteAppointment(Guid appointmentId);
        bool AddAppointment(Appointment newAppointment); 
        bool UpdateAppointment(Appointment updateAppointment);
    }
}