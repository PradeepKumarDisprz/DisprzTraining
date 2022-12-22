using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public interface IAppointmentDAL
    {
        Task<List<Appointment>> GetAppointments();
        void AddAppointments(List<Appointment> appointment);

    }
}