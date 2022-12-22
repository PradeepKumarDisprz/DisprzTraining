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
        public async Task<bool> CheckDateAndTimeFormat(AppointmentDetail updateAppointment)
        {
        if ((updateAppointment.appointmentEndTime <= updateAppointment.appointmentStartTime)||(updateAppointment.appointmentStartTime.Date<DateTime.Now.Date||updateAppointment.appointmentEndTime.Date<DateTime.Now.Date))
            {
                return await Task.FromResult(false);
            }
        return await Task.FromResult(true);
        }

        public async Task<List<Appointment>> GetAppointments()
        {
            return await _appointmentDAL.GetAppointments();
        }

        public async Task<AllAppointments> GetAllAppointments(int offSet, int fetchCount, DateTime? searchDate, string? searchTitle)
        {
            _allAppointments = await _appointmentDAL.GetAppointments();
            var appointmentMatched = _allAppointments.Where(meet => ((searchDate == null) || (meet.appointmentStartTime.Date == searchDate)) && ((searchTitle == null) || (meet.appointmentTitle.ToLower().Contains(searchTitle.ToLower())))).OrderBy(meet=>meet.appointmentStartTime).ToList();
            List<Appointment> truncatedAppointment = new List<Appointment>();
            bool isTruncated = false;
            int appointmentMatchedCount = appointmentMatched.Count();
            //offset 5
            //fetchcount 10---fetches 10 data from 5
            try
            {
                if (appointmentMatched.Any()&&fetchCount!=0)
                {
                   
                    var meetingSkipped = appointmentMatched.Skip(offSet==0?offSet-1:offSet).ToList();
                    if (fetchCount >= meetingSkipped.Count())
                    {
                        truncatedAppointment = meetingSkipped;
                        isTruncated = false;
                    }
                    else
                    {
                        truncatedAppointment = meetingSkipped.Take(fetchCount).ToList();
                        isTruncated = true;
                    }
                }
                else
                {
                    truncatedAppointment=appointmentMatched;
                }
            }
            catch (Exception)
            {
                isTruncated = false;
            }
            AllAppointments appointmentsFound = new AllAppointments()
            {
                isTruncated = isTruncated,
                count = appointmentMatchedCount,
                appointments = truncatedAppointment
            };
            return appointmentsFound;
        }



        //get appointment by Id      
        public async Task<List<Appointment>> GetAppointmentById(Guid appointmentId)
        {
            _allAppointments = await _appointmentDAL.GetAppointments();
            var appointmentById = _allAppointments.Where(meet => meet.appointmentId == appointmentId).ToList();
            return appointmentById;
        }

//list.where
        public async Task<bool> CheckAppointmentConflict(AppointmentDetail appointment)
        {
                _allAppointments = await _appointmentDAL.GetAppointments();
                if (_allAppointments.Count() != 0)
                {
                    foreach (var meet in _allAppointments)
                    {
                    
                        if ((meet.appointmentStartTime < appointment.appointmentEndTime) && (appointment.appointmentStartTime < meet.appointmentEndTime))
                        {
                           return false;
                        }
                    }
                }
                return true;
        }

        // create new appointment
        public async Task<NewAppointmentId?> AddNewAppointment(AppointmentDetail newAppointment)
        {

            var isNoConflict= await CheckAppointmentConflict(newAppointment);

            if(isNoConflict==true)
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


        //update existing appointment
        public async Task<bool> UpdateExistingAppointment(Guid appointmentId, AppointmentDetail updateAppointment)
        {
            
            var isNoConflict = await CheckAppointmentConflict(updateAppointment);
            if (isNoConflict==true)
            {
                _allAppointments = await _appointmentDAL.GetAppointments();
                var isExistingAppointmentDeleted = await DeleteAppointment(appointmentId);
                if (isExistingAppointmentDeleted)
                {
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
            }
            return isNoConflict;   
            }
        }


        //delete Appointment
        public async Task<bool> DeleteAppointment(Guid appointmentId)
        {
            var _allAppointments = await _appointmentDAL.GetAppointments();
            var isDeleted = _allAppointments.Remove(_allAppointments.Where(meet => meet.appointmentId == appointmentId).First());
            if (isDeleted)
            {
                _appointmentDAL.AddAppointments(_allAppointments);
            }
            return isDeleted;
        }
    }
}























