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
        public Task AddAppointments(List<Appointment> appointment)
        {
            _userAppointments=appointment;
            return Task.CompletedTask;
        }

    }
}





















        // public Task<List<AppointmentDetails>> GetAppointmentById(Guid appointmentId)
        // {
        //     var appointmentById = _userAppointments.Where(meet => meet.appointmentId == appointmentId).ToList();
        //     return Task.FromResult(appointmentById);
        // }


        // public Task<NewAppointmentId> AddNewAppointment(Appointment newAppointment)
        // {
        //     var appointmentToBeAdded = new AppointmentDetails
        //     {
        //         appointmentId = Guid.NewGuid(),
        //         appointmentStartTime = newAppointment.appointmentStartTime,
        //         appointmentEndTime = newAppointment.appointmentEndTime,
        //         appointmentTitle = newAppointment.appointmentTitle,
        //         appointmentDescription = newAppointment.appointmentDescription
        //     };

        //     _userAppointments.Add(appointmentToBeAdded);

        //     var createdID = new NewAppointmentId()
        //     {
        //         appointmentId = appointmentToBeAdded.appointmentId
        //     };
        //     return Task.FromResult(createdID);
        // }



        // public Task<bool> UpdateExistingAppointment(Guid appointmentId, Appointment existingAppointment)
        // {
        //     var appointmentToBeUpdated = new AppointmentDetails
        //     {
        //         appointmentId = appointmentId,
        //         appointmentStartTime = existingAppointment.appointmentStartTime,
        //         appointmentEndTime = existingAppointment.appointmentEndTime,
        //         appointmentTitle = existingAppointment.appointmentTitle,
        //         appointmentDescription = existingAppointment.appointmentDescription
        //     };
        //     _userAppointments.Add(appointmentToBeUpdated);
        //     return Task.FromResult(true);
        // }


        // public Task<bool> DeleteAppointment(Guid appointmentId)
        // {
        //     var allAppointments = _userAppointments;
        //     var appointmentMatched = allAppointments.Where(meet => meet.appointmentId == appointmentId).FirstOrDefault();
        //     if (appointmentMatched != null)
        //     {
        //         return Task.FromResult(allAppointments.Remove(appointmentMatched));
        //     }
        //     return Task.FromResult(false);
        // }
    