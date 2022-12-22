using DisprzTraining.Model;


namespace DisprzTraining.Business
{
    public interface IAppointmentBL
    {
         public Task<bool> CheckDateAndTimeFormat(AppointmentDetail updateAppointment);
        Task<List<Appointment>> GetAppointments();
        Task<AllAppointments> GetAllAppointments(int offSet, int fetchCount,DateTime? searchDate,string? searchTitle);
        Task<List<Appointment>> GetAppointmentById(Guid appointmentId);
        Task<NewAppointmentId?> AddNewAppointment(AppointmentDetail newAppointment);
        Task<bool> UpdateExistingAppointment(Guid appointmentId, AppointmentDetail existingAppointment);
        Task<bool> DeleteAppointment(Guid appointmentId);
    }
}