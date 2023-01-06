using DisprzTraining.Model;

namespace DisprzTraining.DataAccess
{
    public class AppointmentDAL : IAppointmentDAL
    {
        private static List<Appointment> _userAppointments = new List<Appointment>()
        {
            new Appointment()
            {
                appointmentId=Guid.NewGuid(),
                appointmentStartTime=DateTime.Now,
                appointmentEndTime=DateTime.Now.AddHours(1),
                appointmentTitle="standup",
                appointmentDescription="meet is going to commence"
            }
        };
        PaginatedAppointments appointmentsFound = new PaginatedAppointments();

        public List<Appointment> GetAppointmentByDate(DateTime date)
        {
            var appointmentMatched = (from appointment in _userAppointments
                                      where (appointment.appointmentStartTime.Date == date)
                                      select appointment).ToList();
            return appointmentMatched;
        }

        public bool DeleteAppointment(Guid appointmentId)
        {
            var appointmentMatched = (from appointment in _userAppointments
                                      where appointment.appointmentId == appointmentId
                                      select appointment).FirstOrDefault();
            if (appointmentMatched != null)
            {
                return _userAppointments.Remove(appointmentMatched);
            }
            return false;
        }

        public bool CheckAppointmentConflict(DateTime startTime, DateTime endTime, List<Appointment> appointments)
        {
            if (endTime == startTime)
            {
                return false;
            }
            var CheckAppointmentPresent = (from appointment in appointments
                                           where (appointment.appointmentStartTime < endTime) && (startTime < appointment.appointmentEndTime)
                                           select appointment).ToList();
            return (CheckAppointmentPresent.Any() ? false : true);
        }

        public NewAppointmentId? AddAppointment(AppointmentDTO newAppointment)
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
                //generating Id for the newAppointment to be added
                var appointmentToBeAdded = new Appointment()
                {
                    appointmentId = Guid.NewGuid(),
                    appointmentStartTime = newAppointment.appointmentStartTime,
                    appointmentEndTime = newAppointment.appointmentEndTime,
                    appointmentTitle = newAppointment.appointmentTitle,
                    appointmentDescription = newAppointment.appointmentDescription
                };
                _userAppointments.Add(appointmentToBeAdded);
                return new() { appointmentId = appointmentToBeAdded.appointmentId };
            }
            return null;
        }

        public bool UpdateAppointment(Guid appointmentId, AppointmentDTO updateAppointment)
        {
            var appointmentsFound = (from appointment in _userAppointments
                                     where (appointment.appointmentId != appointmentId) &&
                                     (appointment.appointmentStartTime.Date == updateAppointment.appointmentStartTime.Date)
                                     select appointment).ToList();
            var noConflict = CheckAppointmentConflict(updateAppointment.appointmentStartTime, updateAppointment.appointmentEndTime, appointmentsFound);
            if (noConflict)
            {
                var deleteAppointment = DeleteAppointment(appointmentId);
                //adding Id to the appointment Model
                var appointmentToBeUpdated = new Appointment()
                {
                    appointmentId = appointmentId,
                    appointmentStartTime = updateAppointment.appointmentStartTime,
                    appointmentEndTime = updateAppointment.appointmentEndTime,
                    appointmentTitle = updateAppointment.appointmentTitle,
                    appointmentDescription = updateAppointment.appointmentDescription
                };
                _userAppointments.Add(appointmentToBeUpdated);
            }
            return noConflict;
        }

        // public async Task<PaginatedAppointments> GetAllAppointments(int offSet, int fetchCount, DateTime? searchDate, string? searchTitle)
        // {

        //     List<Appointment> AppointmentsTaken = new();
        //     bool isTruncated = false;
        //     var appointmentMatched = _userAppointments.Where(meet => ((searchDate == null) || (meet.appointmentStartTime.Date == searchDate)) && ((searchTitle == null) || (meet.appointmentTitle.ToLower().Contains(searchTitle.ToLower())))).OrderBy(meet => meet.appointmentStartTime).ToList();
        //     if (appointmentMatched.Any() && fetchCount > 0)
        //     {
        //         var meetingSkipped = appointmentMatched.Skip(offSet).ToList();
        //         if (fetchCount >= meetingSkipped.Count())
        //         {
        //             AppointmentsTaken = meetingSkipped;
        //         }
        //         else
        //         {
        //             AppointmentsTaken = meetingSkipped.Take(fetchCount).ToList();
        //             isTruncated = true;
        //         }
        //     }
        //     appointmentsFound.appointments = AppointmentsTaken;
        //     appointmentsFound.isTruncated = isTruncated;
        //     return await Task.FromResult(appointmentsFound);
        // }


    }
}




















