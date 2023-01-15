using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {
        private static List<Appointment> _userAppointments = new List<Appointment>();

        public List<Appointment> GetAppointmentByDate(DateTime date)
        {
            var appointmentMatched = (from appointment in _userAppointments
                                      where (appointment.appointmentStartTime.Date == date)
                                      orderby appointment.appointmentStartTime
                                      select appointment).ToList();
            return appointmentMatched;
        }

        private bool CheckAppointmentConflict(DateTime startTime, DateTime endTime, List<Appointment> appointments)
        {
            var CheckAppointmentPresent = (from appointment in appointments
                                           where (appointment.appointmentStartTime < endTime) && (startTime < appointment.appointmentEndTime)
                                           select appointment).ToList();
            return (CheckAppointmentPresent.Any() ? false : true);
        }

        public bool AddAppointment(Appointment newAppointment)
        {
            var noConflict = _userAppointments.Any() ?
                             CheckAppointmentConflict(newAppointment.appointmentStartTime, newAppointment.appointmentEndTime, _userAppointments) : true;
            if (noConflict)
            {
                _userAppointments.Add(newAppointment);
                return true;
            }
            return false;
        }

        public bool DeleteAppointment(Guid appointmentId)
        {
            var appointmentMatched = (from appointment in _userAppointments
                                      where appointment.appointmentId == appointmentId
                                      select appointment).FirstOrDefault();
            return (appointmentMatched != null ? _userAppointments.Remove(appointmentMatched) : false);
        }

        public bool? UpdateAppointment(Appointment updateAppointment)
        {
            var appointmentsFound = (from appointment in _userAppointments
                                     where (appointment.appointmentId != updateAppointment.appointmentId) 
                                     select appointment).ToList();

            var noConflict = appointmentsFound.Count() > 1 ?
                             CheckAppointmentConflict(updateAppointment.appointmentStartTime, updateAppointment.appointmentEndTime, appointmentsFound) : true;

            if (noConflict)
            {
                var deleteAppointment = DeleteAppointment(updateAppointment.appointmentId);
                if (deleteAppointment)
                {
                    _userAppointments.Add(updateAppointment);
                    return true;
                }
                return null;

            }
            return false;
        }
    }
}




















