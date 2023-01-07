using System.ComponentModel.DataAnnotations;
using DisprzTraining.Business;
using DisprzTraining.Model;
using Microsoft.AspNetCore.Mvc;

namespace DisprzTraining.Controllers
{
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentBL _appointmentBL;

        public AppointmentsController(IAppointmentBL appointmentBL)
        {
            _appointmentBL = appointmentBL;
        }

        /// <summary>
        /// Fetch Appointments By Date
        /// </summary>
        ///<remarks>
        /// example
        ///
        ///      date : "2023-01-06"(yyyy-mm-dd)
        ///
        /// </remarks>
        /// <response code="200">Returns list of appointment or empty list if no appointments found</response>
        //- GET /api/appointments/day
        [HttpGet, Route("v1/api/appointments/date")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Appointment>))]
        public ActionResult GetAppointmentByDate([FromQuery] DateTime date)
        {
            var appointmentFound = _appointmentBL.GetAppointmentByDate(date);
            return Ok(appointmentFound);
        }



        /// <summary>
        /// Add new Appointment
        /// </summary>
        /// <param name="newAppointment"></param>
        /// <remarks>
        /// example:
        ///
        ///     {
        ///        "appointmentStartTime": "2023-01-06T20:43:33.005Z",
        ///        "appointmentEndTime": "2023-01-06T20:44:43.005Z",
        ///        "appointmentTitle": "string",
        ///        "appointmentDescription":"string"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created appointmentId</response>
        /// <response code="400">Bad request if starttime greater than end time</response>
        /// <response code="409">Conflict Occured between meetings</response>
        //- POST /api/appointments
        [HttpPost, Route("v1/api/appointments")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NewAppointmentId))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public ActionResult AddAppointment([FromBody] AppointmentDTO newAppointment)
        {
            if ((newAppointment.appointmentEndTime <= newAppointment.appointmentStartTime)
               || (newAppointment.appointmentStartTime < DateTime.Now))
            {
                return BadRequest(APIResponse.BadRequestResponse);
            }
            var result = _appointmentBL.AddAppointment(newAppointment);
            return (result != null) ? Created(nameof(GetAppointmentByDate), result) : Conflict(APIResponse.ConflictResponse);
        }



        /// <summary>
        /// Delete an appointment
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <remarks>
        /// example:
        ///
        ///        Id: "2ef43h26-4524-5245-g56a-5d552v96h1f6",
        ///
        /// </remarks>
        /// <response code="204">Deletes an appointment successfully</response>
        /// <response code="404">Appointment is not found</response>
        // DELETE /api/appointments/{Id}
        [HttpDelete, Route("v1/api/appointments/{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public ActionResult DeleteAppointment([FromRoute] Guid Id)
        {
            var isDeleted = _appointmentBL.DeleteAppointment(Id);
            return (isDeleted) ? NoContent() : NotFound(APIResponse.NotFoundResponse);
        }


        /// <summary>
        /// Update an Existing appointment
        /// </summary>
        /// <param name="updateAppointment"></param>
        /// <param name="Id"></param>
        /// <remarks>
        /// example:
        ///
        ///     Id: "2ef43h26-4524-5245-g56a-5d552v96h1f6",
        ///     {
        ///        
        ///        "appointmentStartTime": "2023-01-06T20:43:33.005Z",
        ///        "appointmentEndTime": "2023-01-06T20:44:43.005Z",
        ///        "appointmentTitle": "string",
        ///        "appointmentDescription":"string"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">No Content if appointment updated</response>
        /// <response code="400">Bad request if starttime greater than end time</response>
        /// <response code="409">Conflict Occured between meetings</response>
        [HttpPut, Route("v1/api/appointments/{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public ActionResult UpdateAppointment([FromRoute] Guid Id, [FromBody] AppointmentDTO updateAppointment)
        {
            if ((updateAppointment.appointmentEndTime <= updateAppointment.appointmentStartTime)
                || (updateAppointment.appointmentStartTime < DateTime.Now))
            {
                return BadRequest(APIResponse.BadRequestResponse);
            }
            bool noConflict = _appointmentBL.UpdateAppointment(Id, updateAppointment);
            return (noConflict) ? NoContent() : Conflict(APIResponse.ConflictResponse);
        }
 




// ///for learning purpose/// //

/// <summary>
/// Fetch All Appointments with Date/Title with offset and fetchCount
/// </summary>
[HttpGet, Route("v1/api/appointments")]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedAppointments))]
public ActionResult GetAllAppointments([Required] int offSet = 0, [Required] int fetchCount = 10, [DataType(DataType.Date)] DateTime? searchDate = null, string? searchTitle = null)
{
    var appointments =  _appointmentBL.GetAllAppointments(offSet, fetchCount, searchDate, searchTitle);
    return Ok(appointments);
}



   }
}
