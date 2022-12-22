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
                    if (offSet > 0)
                    {
                        offSet -= 1;
                    }

                    var meetingSkipped = appointmentMatched.Skip(offSet).ToList();
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


        public async Task<bool> CheckCreateAppointmentConflict(AppointmentDetail appointment)
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

             if ((newAppointment.appointmentEndTime <= newAppointment.appointmentStartTime)||(newAppointment.appointmentStartTime.Date<DateTime.Now.Date||newAppointment.appointmentEndTime.Date<DateTime.Now.Date))
            {
                throw new Exception();
            }
            var isNoConflict= await CheckCreateAppointmentConflict(newAppointment);

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


        public async Task<bool> CheckUpdateAppointmentConflict(Guid appointmentId, AppointmentDetail appointment)
        {
                _allAppointments = await _appointmentDAL.GetAppointments();
                foreach (var meet in _allAppointments)
                {
                    if (meet.appointmentId == appointmentId)
                    {
                        continue;
                    }
                    else if ((meet.appointmentStartTime < appointment.appointmentEndTime) && (appointment.appointmentStartTime < meet.appointmentEndTime))
                    {
                        
                        return false;
                    }
                }
                return true;
        }


        //update existing appointment
        public async Task<bool> UpdateExistingAppointment(Guid appointmentId, AppointmentDetail updateAppointment)
        {
            if ((updateAppointment.appointmentEndTime <= updateAppointment.appointmentStartTime)||(updateAppointment.appointmentStartTime.Date<DateTime.Now.Date||updateAppointment.appointmentEndTime.Date<DateTime.Now.Date))
            {
                throw new Exception();
            }

            var isNoConflict = await CheckUpdateAppointmentConflict(appointmentId, updateAppointment);
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
































// //get all appointments
//         public async Task<List<AppointmentDetails>>? GetAppointments()
//         {
//             return await _appointmentDAL.GetAppointments();
//         }


// //offset fetchcount
//         public Task<TruncatedModel>? GetAppointments(int offSet, int fetchCount)
//         {
//             var allAppointments = AppointmentList.userAppointments;
//             bool truncated = true;
//             TruncatedModel appointment = new TruncatedModel();

//             if (offSet > allAppointments.Count())
//             {
//                 truncated = false;
//                 appointment.isTruncated = truncated;
//             }
//             else if ((offSet < allAppointments.Count() && offSet != allAppointments.Count() && fetchCount <= allAppointments.Count()))
//             {
//                 //offset should be less than the count of meets 
//                 //else all meets will be skipped and no content will be displayed
//                 try
//                 {
//                     var truncatedAppointment = allAppointments.GetRange(offSet, fetchCount);
//                     if (truncatedAppointment.Any())
//                     {
//                         appointment.isTruncated = truncated;
//                         appointment.Appointments = truncatedAppointment;
//                     }
//                 }
//                 catch (Exception)
//                 {

//                 }
//             }
//             return Task.FromResult(appointment);
//         }


// //appointmentDate offset fetchcount
//         public Task<TruncatedModel>? GetAppointments(DateTime? appointmentDate, int offSet, int fetchCount)
//         {
//             var allAppointments = AppointmentList.userAppointments;
//             var appointmentByDate = allAppointments.Where(meet => meet.appointmentStartTime.Date == appointmentDate).OrderBy(meet => meet.appointmentStartTime).ToList();
//             bool truncated = true;
//             TruncatedModel appointment = new TruncatedModel();

//             if (offSet > appointmentByDate.Count())
//             {
//                 truncated = false;
//                 appointment.isTruncated = truncated;
//             }
//             else if ((offSet < appointmentByDate.Count() && offSet != appointmentByDate.Count() && fetchCount <= appointmentByDate.Count()))
//             {
//                 //offset should be less than the count of meets 
//                 //else all meets will be skipped and no content will be displayed
//                 try
//                 {
//                     var truncatedAppointment = appointmentByDate.GetRange(offSet, fetchCount);
//                     if (truncatedAppointment.Any())
//                     {
//                         appointment.isTruncated = truncated;
//                         appointment.Appointments = truncatedAppointment;
//                     }
//                 }
//                 catch (Exception)
//                 {

//                 }
//             }
//             return Task.FromResult(appointment);
//         }

// //title offset fetchcount
//         public Task<TruncatedModel>? GetAppointments(string? title, int offSet, int fetchCount)
//         {
//             var allAppointments = AppointmentList.userAppointments;
//             var appointmentByTitle = allAppointments.Where(meet => meet.appointmentTitle == title).ToList();
//             bool truncated = true;
//             TruncatedModel appointment = new TruncatedModel();

//             if (offSet > appointmentByTitle.Count())
//             {
//                 truncated = false;
//                 appointment.isTruncated = truncated;
//             }
//             else if ((offSet < appointmentByTitle.Count() && offSet != appointmentByTitle.Count() && fetchCount <= appointmentByTitle.Count()))
//             {
//                 //offset should be less than the count of meets 
//                 //else all meets will be skipped and no content will be displayed
//                 try
//                 {
//                     var truncatedAppointment = appointmentByTitle.GetRange(offSet, fetchCount);
//                     if (truncatedAppointment.Any())
//                     {
//                         appointment.isTruncated = truncated;
//                         appointment.Appointments = truncatedAppointment;
//                     }
//                 }
//                 catch (Exception)
//                 {

//                 }
//             }
//             return Task.FromResult(appointment);
//         }

// //appointmentDate title offset fetchcount

//         public Task<TruncatedModel>? GetAppointments(DateTime? appointmentDate,string? title, int offSet, int fetchCount)
//         {
//             var allAppointments = AppointmentList.userAppointments;
//             var appointmentByDateAndTitle = allAppointments.Where(meet => (meet.appointmentStartTime.Date == appointmentDate) && (meet.appointmentTitle == title)).OrderBy(meet => meet.appointmentStartTime).ToList();
//             bool truncated = true;
//             TruncatedModel appointment = new TruncatedModel();

//             if (offSet > appointmentByDateAndTitle.Count())
//             {
//                 truncated = false;
//                 appointment.isTruncated = truncated;
//             }
//             else if ((offSet < appointmentByDateAndTitle.Count() && offSet != appointmentByDateAndTitle.Count() && fetchCount <= appointmentByDateAndTitle.Count()))
//             {
//                 //offset should be less than the count of meets 
//                 //else all meets will be skipped and no content will be displayed
//                 try
//                 {
//                     var truncatedAppointment = appointmentByDateAndTitle.GetRange(offSet, fetchCount);
//                     if (truncatedAppointment.Any())
//                     {
//                         appointment.isTruncated = truncated;
//                         appointment.Appointments = truncatedAppointment;
//                     }
//                 }
//                 catch (Exception)
//                 {

//                 }
//             }
//             return Task.FromResult(appointment);
//         }





























// //get appointment by date with counts and offset
// public Task<TruncatedModel>? GetAppointments(DateTime? appointmentDate, int offSet, int fetchCount)
// {
//     var allAppointments = AppointmentList.userAppointments;

//     if (allAppointments.Any())
//     {
//         var appointmentMatchedByDate = from meet in allAppointments
//                                        where meet.appointmentStartTime.Date == appointmentDate//optional
//                                        orderby meet.appointmentStartTime
//                                        select meet;


//         if (appointmentMatchedByDate.Any())
//         {
//             if ((offSet < appointmentMatchedByDate.Count()))
//             {
//                 //offset should be less than the count of meets 
//                 //else all meets will be skipped and no content will be displayed
//                 var truncatedAppointment = appointmentMatchedByDate.Skip(offSet).Take(fetchCount).ToList();
//                 if (truncatedAppointment.Any())
//                 {
//                     TruncatedModel appointmentTruncatedByDate = new TruncatedModel()
//                     {
//                         isTruncated = true,
//                         Appointments = truncatedAppointment
//                     };
//                     return Task.FromResult(appointmentTruncatedByDate);
//                 }
//             }
//         }
//     }


//     return null;
// }



// if (((appointment.appointmentStartTime >= meet.appointmentStartTime) && (appointment.appointmentStartTime < meet.appointmentEndTime)) ||
//     (appointment.appointmentEndTime > meet.appointmentStartTime) && (appointment.appointmentEndTime < meet.appointmentEndTime) ||
//     (appointment.appointmentStartTime <= meet.appointmentStartTime) && (appointment.appointmentEndTime >= meet.appointmentEndTime))

// if (((appointment.appointmentStartTime >= meet.appointmentStartTime) && (appointment.appointmentStartTime <= meet.appointmentEndTime)) ||
//     (appointment.appointmentEndTime >= meet.appointmentStartTime) && (appointment.appointmentEndTime <= meet.appointmentEndTime) ||
//     (appointment.appointmentStartTime <= meet.appointmentStartTime) && (appointment.appointmentEndTime >= meet.appointmentEndTime))








// //get by date
// public Task<List<AppointmentDetails>>? GetAppointments(DateTime? appointmentDate)
// {
//     var allAppointments = AppointmentList.userAppointments;
//     var appointmentByDate = allAppointments.Where(meet => meet.appointmentStartTime.Date == appointmentDate).OrderBy(meet => meet.appointmentStartTime).ToList();
//     return Task.FromResult(appointmentByDate);
// // }



// public Task<List<AppointmentDetails>>? GetAppointments(string? title)
// {
//     var allAppointments = AppointmentList.userAppointments;
//     var appointmentByTitle = allAppointments.Where(meet => meet.appointmentTitle == title).ToList();
//     return Task.FromResult(appointmentByTitle);
// }

// public Task<List<AppointmentDetails>>? GetAppointments(DateTime? appointmentDate, string? title)
// {
//     var allAppointments = AppointmentList.userAppointments;
//     var appointmentByDatenTitle = allAppointments.Where(meet => (meet.appointmentStartTime.Date == appointmentDate) && (meet.appointmentTitle == title)).OrderBy(meet => meet.appointmentStartTime).ToList();
//     return Task.FromResult(appointmentByDatenTitle);
// }