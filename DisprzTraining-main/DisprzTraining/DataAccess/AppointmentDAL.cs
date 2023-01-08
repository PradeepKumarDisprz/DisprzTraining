using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {
        private static List<Appointment> _userAppointments = new List<Appointment>()
        {
            new Appointment()
            {
                appointmentId=Guid.Parse("37981518-40f1-4580-946b-d47eb379453e"),
                appointmentStartTime = new DateTime(2023, 01, 06, 13, 30, 00),
                appointmentEndTime = new DateTime(2023, 01, 06, 14, 30, 00),
                appointmentTitle="standup",
                appointmentDescription="meet is going to commence"
            },
            new Appointment()
            {
                appointmentId=Guid.Parse("27921518-40f1-4580-946b-d47eb379453e"),
                appointmentStartTime = new DateTime(2023, 01, 06, 14, 30, 00),
                appointmentEndTime = new DateTime(2023, 01, 06, 15, 30, 00),
                appointmentTitle="standup",
                appointmentDescription="meet is going to commence"
            }
        };
        PaginatedAppointments appointmentsFound = new PaginatedAppointments();

        public List<Appointment> GetAppointmentByDate(DateTime date)
        {
            var appointmentMatched = (from appointment in _userAppointments
                                      where (appointment.appointmentStartTime.Date == date)
                                      orderby appointment.appointmentStartTime
                                      select appointment).ToList();
            return appointmentMatched;
        }

        public bool CheckAppointmentConflict(DateTime startTime, DateTime endTime, List<Appointment> appointments)
        {
            var CheckAppointmentPresent = (from appointment in appointments
                                           where (appointment.appointmentStartTime < endTime) && (startTime < appointment.appointmentEndTime)
                                           select appointment).ToList();
            return (CheckAppointmentPresent.Any() ? false : true);
        }

        public bool AddAppointment(Appointment newAppointment)
        {
            //gives appointments matching the date of the new appointment
            var appointmentsFound = (from appointment in _userAppointments
                                     where (appointment.appointmentStartTime.Date == newAppointment.appointmentStartTime.Date)
                                     select appointment).ToList();

            var noConflict = appointmentsFound.Any() ?
                             CheckAppointmentConflict(newAppointment.appointmentStartTime, newAppointment.appointmentEndTime, appointmentsFound)
                             : true;
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

        public bool UpdateAppointment(Appointment updateAppointment)
        {
            var appointmentsFound = (from appointment in _userAppointments
                                     where (appointment.appointmentId != updateAppointment.appointmentId) &&
                                     (appointment.appointmentStartTime.Date == updateAppointment.appointmentStartTime.Date)
                                     select appointment).ToList();

            var noConflict = appointmentsFound.Count() > 1 ?
                             CheckAppointmentConflict(updateAppointment.appointmentStartTime, updateAppointment.appointmentEndTime, appointmentsFound)
                             : true;

            if (noConflict)
            {
                var deleteAppointment = DeleteAppointment(updateAppointment.appointmentId);
                _userAppointments.Add(updateAppointment);
                return true;
            }
            return false;
        }


        //for learning purpose
        public PaginatedAppointments GetAllAppointments(int offSet, int fetchCount, DateTime? searchDate, string? searchTitle)
        {

            var appointmentMatched = _userAppointments.Where(meet => ((searchDate == null) || (meet.appointmentStartTime.Date == searchDate)) && ((searchTitle == null) || (meet.appointmentTitle.ToLower().Contains(searchTitle.ToLower())))).OrderBy(meet => meet.appointmentStartTime).ToList();
            if (appointmentMatched.Any() && fetchCount > 0)
            {
                appointmentsFound.appointments = appointmentMatched.Skip(offSet).Take(fetchCount).ToList();
                appointmentsFound.isTruncated = fetchCount >= appointmentMatched.Skip(offSet).Count() ? false : true;
            }
            else
            {
                appointmentsFound.appointments = appointmentMatched;
                appointmentsFound.isTruncated = false;
            }

            return appointmentsFound;
        }
    }
}




















