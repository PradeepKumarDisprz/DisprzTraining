using DisprzTraining.DataAccess;
using DisprzTraining.Model;

namespace DisprzTraining.Business
{
    public class AppointmentBL : IAppointmentBL
    {
        private readonly IAppointmentDAL _appointmentDAL;

        public AppointmentBL(IAppointmentDAL appointmentDAL)
        {
            _appointmentDAL = appointmentDAL;
        }

        // get appointment by date
        public List<Appointment> GetAppointmentByDate(DateTime date)
        {
            return _appointmentDAL.GetAppointmentByDate(date);
        }

        // create new appointment
        public NewAppointmentId? AddAppointment(AppointmentDTO newAppointment)
        {
            //generating Id for the newAppointment to be added
            Appointment appointmentToBeAdded = new()
            {
                appointmentId = Guid.NewGuid(),
                appointmentStartTime = newAppointment.appointmentStartTime,
                appointmentEndTime = newAppointment.appointmentEndTime,
                appointmentTitle = newAppointment.appointmentTitle,
                appointmentDescription = newAppointment.appointmentDescription
            };
            var isCreated = _appointmentDAL.AddAppointment(appointmentToBeAdded);
            return isCreated ? new() { Id = appointmentToBeAdded.appointmentId } : null;
        }

        //delete Appointment
        public bool DeleteAppointment(Guid appointmentId)
        {
            return _appointmentDAL.DeleteAppointment(appointmentId);
        }

        public bool? UpdateAppointment(Guid appointmentId, AppointmentDTO updateAppointment)
        {
            Appointment appointmentToBeUpdated = new()
            {
                appointmentId = appointmentId,
                appointmentStartTime = updateAppointment.appointmentStartTime,
                appointmentEndTime = updateAppointment.appointmentEndTime,
                appointmentTitle = updateAppointment.appointmentTitle,
                appointmentDescription = updateAppointment.appointmentDescription
            };
            bool? isUpdated = _appointmentDAL.UpdateAppointment(appointmentToBeUpdated);
            return isUpdated!=null ?( isUpdated==true? true : false):null;
        }

    }
}























