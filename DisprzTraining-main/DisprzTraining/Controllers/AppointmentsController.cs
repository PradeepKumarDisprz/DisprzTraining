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
        [HttpGet, Route("v1/api/appointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Appointment>))]
        public ActionResult GetAppointmentByDate([FromQuery] DateTime date)
        {
            var appointmentFound = _appointmentBL.GetAppointmentByDate(date);
            return Ok(appointmentFound);
        }

        /// <summary>
        /// Post a new Appointment
        /// </summary>
        [HttpPost, Route("v1/api/appointments")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NewAppointmentId))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public ActionResult AddAppointment([FromBody] AppointmentDTO newAppointment)
        {
            try
            {
                var appointmentId = _appointmentBL.AddAppointment(newAppointment);
                return (appointmentId != null) ? Created("", appointmentId) : Conflict(APIResponse.ConflictResponse);
            }
            catch (Exception)
            {
                return BadRequest(APIResponse.BadRequestResponse);
            }
        }

        /// <summary>
        /// Delete an appointment
        /// </summary>
        [HttpDelete, Route("v1/api/appointments/{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponse))]
        public ActionResult DeleteAppointment([FromRoute] Guid appointmentId)
        {
            var isDeleted = _appointmentBL.DeleteAppointment(appointmentId);
            return (isDeleted) ? NoContent() : NotFound(APIResponse.NotFoundResponse);
        }
        
        
        
        


        ///for learning purpose///

        /// <summary>
        /// Update an Existing appointment
        /// </summary>
        [HttpPut, Route("v1/api/appointments/{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        public ActionResult UpdateAppointment([FromRoute] Guid appointmentId, [FromBody] AppointmentDTO updateAppointment)
        {
            try
            {
                bool noConflict = _appointmentBL.UpdateAppointment(appointmentId, updateAppointment);
                return (noConflict) ? NoContent() : Conflict(APIResponse.ConflictResponse);
            }
            catch (Exception)
            {
                return BadRequest(APIResponse.BadRequestResponse);
            }
        }

        // /// <summary>
        // /// Fetch All Appointments with Date/Title with offset and fetchCount
        // /// </summary>
        // [HttpGet, Route("v1/appointments")]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedAppointments))]
        // public async Task<ActionResult> GetAllAppointments([Required] int offSet = 0, [Required] int fetchCount = 10, [DataType(DataType.Date)] DateTime? searchDate = null, string? searchTitle = null)
        // {
        //     var appointments = await _appointmentBL.GetAllAppointments(offSet, fetchCount, searchDate, searchTitle);
        //     return Ok(appointments);
        // }

        // /// <summary>
        // /// Fetch All Appointments with Date/Title with offset and fetchCount
        // /// </summary>
        // [HttpGet, Route("v1/appointments")]
        // [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginatedAppointments))]
        // public async Task<ActionResult> GetAllAppointments([Required] int offSet = 0, [Required] int fetchCount = 10, [DataType(DataType.Date)] DateTime? searchDate = null, string? searchTitle = null)
        // {
        //     var appointments = await _appointmentBL.GetAllAppointments(offSet, fetchCount, searchDate, searchTitle);
        //     return Ok(appointments);
        // }

    }
}



