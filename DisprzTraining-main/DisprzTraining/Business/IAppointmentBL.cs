using DisprzTraining.Model;


namespace DisprzTraining.Business
{
    public interface IAppointmentBL
    {
        List<Appointment> GetAppointmentByDate(DateTime date);
        NewAppointmentId? AddAppointment(AppointmentDTO newAppointment);
        bool DeleteAppointment(Guid appointmentId);
        bool UpdateAppointment(Guid appointmentId, AppointmentDTO existingAppointment);
        // Task<PaginatedAppointments> GetAllAppointments(int offSet, int fetchCount,DateTime? searchDate,string? searchTitle);
    }
}