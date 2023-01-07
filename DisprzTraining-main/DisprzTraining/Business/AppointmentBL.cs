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

        public bool UpdateAppointment(Guid appointmentId, AppointmentDTO updateAppointment)
        {
            Appointment appointmentToBeUpdated = new()
            {
                appointmentId = appointmentId,
                appointmentStartTime = updateAppointment.appointmentStartTime,
                appointmentEndTime = updateAppointment.appointmentEndTime,
                appointmentTitle = updateAppointment.appointmentTitle,
                appointmentDescription = updateAppointment.appointmentDescription
            };
            var isUpdated = _appointmentDAL.UpdateAppointment(appointmentToBeUpdated);
            return isUpdated ? true : false;
        }


        public PaginatedAppointments GetAllAppointments(int offSet, int fetchCount, DateTime? searchDate, string? searchTitle)
        {
            return  _appointmentDAL.GetAllAppointments(offSet, fetchCount, searchDate, searchTitle);
        }
    }
}



//for learning purpose

// public async Task<bool> UpdateAppointment(Guid appointmentId, AppointmentDTO updateAppointment)
// {
//     if ((updateAppointment.appointmentEndTime < updateAppointment.appointmentStartTime) || (updateAppointment.appointmentStartTime.Date < DateTime.Now.Date))
//     {
//         throw new Exception();
//     }
//     return (await _appointmentDAL.UpdateAppointment(appointmentId, updateAppointment));
// }

// public async Task<PaginatedAppointments> GetAllAppointments(int offSet, int fetchCount, DateTime? searchDate,string? searchTitle)
// { 
//     return (await _appointmentDAL.GetAllAppointments(offSet, fetchCount, searchDate, searchTitle));
// }


























