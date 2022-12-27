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

        List<Appointment>? _allAppointments;

        public async Task<AllAppointments> GetAllAppointments(int offSet, int fetchCount, DateTime? searchDate, string? searchTitle)
        {
            _allAppointments = await _appointmentDAL.GetAppointments();
            var appointmentMatched = _allAppointments.Where(meet => ((searchDate == null) || (meet.appointmentStartTime.Date == searchDate)) && ((searchTitle == null) || (meet.appointmentTitle.ToLower().Contains(searchTitle.ToLower())))).OrderBy(meet => meet.appointmentStartTime).ToList();
           
            List<Appointment> AppointmentsTaken= new();
            bool isTruncated = false;
            int appointmentMatchedCount = appointmentMatched.Count();
            
            //offset 5
            //fetchcount 10---fetches 10 data from 5
            if (appointmentMatched.Any() && fetchCount != 0)
            {

                var meetingSkipped = appointmentMatched.Skip(offSet == 0 ? offSet - 1 : offSet).ToList();
                if (fetchCount >= meetingSkipped.Count())
                {
                    AppointmentsTaken = meetingSkipped;
                    // isTruncated = false;
                }
                else
                {
                    AppointmentsTaken = meetingSkipped.Take(fetchCount).ToList();
                    isTruncated = true;
                }
            }
            else if(searchDate!=null&&searchTitle==null)
            {
                
                AppointmentsTaken = appointmentMatched;
            }
            AllAppointments appointmentsFound = new AllAppointments()
            {
                isTruncated = isTruncated,
                count = appointmentMatchedCount,
                appointments = AppointmentsTaken
            };
            return appointmentsFound;
        }



        //get appointment by Id      
        public async Task<Appointment?> GetAppointmentById(Guid appointmentId)
        {
            _allAppointments = await _appointmentDAL.GetAppointments();
            var appointmentById = _allAppointments.Where(meet => meet.appointmentId == appointmentId);
            
            if (appointmentById.Any())
            {
                return appointmentById.First();
            }
            return null;
        }


        public async Task<bool> CheckPastDateAndTime(AppointmentDTO appointment)
        {
            if ((appointment.appointmentEndTime < appointment.appointmentStartTime) || (appointment.appointmentStartTime.Date < DateTime.Now.Date || appointment.appointmentEndTime.Date < DateTime.Now.Date))
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }


        public async Task<bool> CheckAppointmentConflict(AppointmentDTO appointment)
        {
            if (appointment.appointmentEndTime == appointment.appointmentStartTime)
            {
                return false;
            }
            _allAppointments = await _appointmentDAL.GetAppointments();
            if (_allAppointments.Count() != 0)
            {
                var CheckAppointmentPresent = _allAppointments.Where(meet => (meet.appointmentStartTime < appointment.appointmentEndTime) && (appointment.appointmentStartTime < meet.appointmentEndTime));//add = if conflict needed for 1:40:10 to 2:30:10 and 2:30:10 to 4:20:10
                if (CheckAppointmentPresent.Any())
                {
                    return false;
                }
            }
            return true;
        }

        // create new appointment
        public async Task<NewAppointmentId?> AddNewAppointment(AppointmentDTO newAppointment)
        {

            var isNoConflict = await CheckAppointmentConflict(newAppointment);

            if (isNoConflict == true)
            {
                _allAppointments = await _appointmentDAL.GetAppointments();
                //generating Id for the newAppointment to be added
                var appointmentToBeAdded = new Appointment()
                {
                    appointmentId = Guid.NewGuid(),
                    appointmentStartTime = newAppointment.appointmentStartTime,
                    appointmentEndTime = newAppointment.appointmentEndTime,
                    appointmentTitle = newAppointment.appointmentTitle,
                    appointmentDescription = newAppointment.appointmentDescription
                };
                var newAppointmentId = new NewAppointmentId()
                {
                    appointmentId = appointmentToBeAdded.appointmentId
                };
                _allAppointments.Add(appointmentToBeAdded);
                _appointmentDAL.AddAppointments(_allAppointments);
                return newAppointmentId;
            }
            return null;
        }

        public async Task<bool> CheckAppointmentConflict(Guid appointmentId, AppointmentDTO appointment)
        {
            if (appointment.appointmentEndTime == appointment.appointmentStartTime)
            {
                return false;
            }
            _allAppointments = await _appointmentDAL.GetAppointments();

            var CheckAppointmentPresent = _allAppointments.Where((meet => meet.appointmentId != appointmentId && (meet.appointmentStartTime < appointment.appointmentEndTime) && (appointment.appointmentStartTime < meet.appointmentEndTime)));//add = if conflict needed for 1:40:10 to 2:30:10 and 2:30:10 to 4:20:10
            if (CheckAppointmentPresent.Any())
            {
                return false;
            }

            return true;
        }
        //gets deleted b4 conflict ==what if user dontupdates if conflict occurs(meet is deleted so it won't show)

        //update existing appointment
        public async Task<bool> UpdateExistingAppointment(Guid appointmentId, AppointmentDTO updateAppointment)
        {
            var isNoConflict = await CheckAppointmentConflict(appointmentId, updateAppointment);
            if (isNoConflict)
            {
                var deleteAppointment = DeleteAppointment(appointmentId);
                _allAppointments = await _appointmentDAL.GetAppointments();
                //adding Id to the appointment Model
                var appointmentToBeUpdated = new Appointment()
                {
                    appointmentId = appointmentId,
                    appointmentStartTime = updateAppointment.appointmentStartTime,
                    appointmentEndTime = updateAppointment.appointmentEndTime,
                    appointmentTitle = updateAppointment.appointmentTitle,
                    appointmentDescription = updateAppointment.appointmentDescription
                };
                _allAppointments.Add(appointmentToBeUpdated);
                _appointmentDAL.AddAppointments(_allAppointments);
            }
            return isNoConflict;
        }


        //delete Appointment
        public async Task<bool> DeleteAppointment(Guid appointmentId)
        {
            _allAppointments = await _appointmentDAL.GetAppointments();
            var appointmentMatched = _allAppointments.Where(meet => meet.appointmentId == appointmentId);
            var isDeleted = false;

            if (appointmentMatched.Any())
            {
                isDeleted = _allAppointments.Remove(appointmentMatched.First());
                if (isDeleted)
                {
                   await _appointmentDAL.AddAppointments(_allAppointments);
                }
            }
            return isDeleted;
        }
    }


}





















