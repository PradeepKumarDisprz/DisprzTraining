using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        // Task<PaginatedAppointments> GetAllAppointments(int offSet, int fetchCount,DateTime? startDate,string? searchTitle);
        List<Appointment> GetAppointmentByDate(DateTime date);
        bool DeleteAppointment(Guid appointmentId);
        NewAppointmentId? AddAppointment(AppointmentDTO newAppointment); 
        bool UpdateAppointment(Guid appointmentId, AppointmentDTO updateAppointment);
    }
}