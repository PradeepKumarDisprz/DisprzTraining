using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        PaginatedAppointments GetAllAppointments(int offSet, int fetchCount,DateTime? startDate,string? searchTitle);
        List<Appointment> GetAppointmentByDate(DateTime date);
        bool DeleteAppointment(Guid appointmentId);
        bool AddAppointment(Appointment newAppointment); 
        bool UpdateAppointment(Appointment updateAppointment);
    }
}