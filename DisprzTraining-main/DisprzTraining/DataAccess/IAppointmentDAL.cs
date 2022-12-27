using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        Task<List<Appointment>> GetAppointments();
        Task AddAppointments(List<Appointment> appointment);

    }
}