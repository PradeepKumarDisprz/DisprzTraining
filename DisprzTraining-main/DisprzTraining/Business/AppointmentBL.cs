using System.Net.Http;
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

        public List<Appointment> GetAppointmentByDate(DateTime date)
        {
            return _appointmentDAL.GetAppointmentByDate(date);
        }

        // create new appointment
        public NewAppointmentId? AddAppointment(AppointmentDTO newAppointment)
        {
            if ((newAppointment.appointmentEndTime < newAppointment.appointmentStartTime)
                || (newAppointment.appointmentStartTime < DateTime.Now))
            {
                throw new Exception();
            }
            return _appointmentDAL.AddAppointment(newAppointment);
        }

        //delete Appointment
        public bool DeleteAppointment(Guid appointmentId)
        {
            return _appointmentDAL.DeleteAppointment(appointmentId);
        }


        //for learning purpose
        public bool UpdateAppointment(Guid appointmentId, AppointmentDTO updateAppointment)
        {
            if ((updateAppointment.appointmentEndTime < updateAppointment.appointmentStartTime) 
               || (updateAppointment.appointmentStartTime.Date < DateTime.Now.Date))
            {
                throw new Exception();
            }
            return  _appointmentDAL.UpdateAppointment(appointmentId, updateAppointment);
        }
    }
}





// throw(); 406 Not accepted
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


























