using DisprzTraining.Model;


namespace DisprzTraining.Business
{
    public interface IAppointmentBL
    {
        Task<bool> CheckPastDateAndTime(AppointmentDTO updateAppointment);


        Task<AllAppointments> GetAllAppointments(int offSet, int fetchCount,DateTime? searchDate,string? searchTitle);
        Task<Appointment?> GetAppointmentById(Guid appointmentId);
        Task<NewAppointmentId?> AddNewAppointment(AppointmentDTO newAppointment);
        Task<bool> UpdateExistingAppointment(Guid appointmentId, AppointmentDTO existingAppointment);
        Task<bool> DeleteAppointment(Guid appointmentId);
    }
}