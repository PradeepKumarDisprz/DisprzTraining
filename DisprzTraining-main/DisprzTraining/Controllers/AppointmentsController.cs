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

        //get all appointments
        [HttpGet, Route("v1/appointments")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AllAppointments))]
        public async Task<ActionResult> GetAllAppointments([Required] int offSet = 0, [Required] int fetchCount = 10, DateTime? searchDate = null, string? searchTitle = null)
        {
            var appointments = await _appointmentBL.GetAllAppointments(offSet, fetchCount, searchDate, searchTitle);
            return Ok(appointments);
        }

        //get appointment by Id
        [HttpGet, Route("v1/appointments/{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Appointment))]
        public async Task<ActionResult> GetappointmentById([FromRoute] Guid appointmentId)
        {
            var existingAppointmentById = await _appointmentBL.GetAppointmentById(appointmentId);
            if (existingAppointmentById != null)
            {
                return Ok(existingAppointmentById);
            }
            return NotFound(new ErrorMessage()
            {
                errorMessage = "No Appointment found with the given Id",
                errorCode = 404
            });
        }

        //add new appointment
        [HttpPost, Route("v1/appointments")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NewAppointmentId))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorMessage))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
        public async Task<ActionResult> AddNewAppointment([FromBody] AppointmentDTO newAppointment)
        {
            if (await _appointmentBL.CheckPastDateAndTime(newAppointment))
            {
                var newAppointmentId = await _appointmentBL.AddNewAppointment(newAppointment);
                if (newAppointmentId != null)
                {
                    var actionName = nameof(GetappointmentById);
                    return Created(actionName, newAppointmentId);
                }
                else
                {
                    return Conflict(new ErrorMessage()
                    {
                        errorMessage = "Conflict occured between same meeting time or two different meetings",
                        errorCode = 409
                    });
                }
            }
            return BadRequest(new ErrorMessage()
            {
                errorMessage = "Appointment for past time and days are not allowed ",
                errorCode = 400
            });
        }

        //update existing appointment
        [HttpPut, Route("v1/appointments/{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorMessage))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
        public async Task<ActionResult> UpdateExistingAppointment([FromRoute] Guid appointmentId, [FromBody] AppointmentDTO updateAppointment)
        {
            var isDateTimeCorrect=await _appointmentBL.CheckPastDateAndTime(updateAppointment);
            if (isDateTimeCorrect)
            {
                var appointmentPresent =await _appointmentBL.GetAppointmentById(appointmentId);
                if (appointmentPresent!=null)
                {
                    bool isNoConflict = await _appointmentBL.UpdateExistingAppointment(appointmentId, updateAppointment);
                    if (isNoConflict)
                    {
                        return NoContent();
                    }
                    else
                    {
                        return Conflict(new ErrorMessage()
                        {
                            errorMessage = "Conflict occured between same meeting time or two different meetings",
                            errorCode = 409
                        });
                    }
                }
                return NotFound(new ErrorMessage()
                {
                    errorMessage = "No Appointment found with the given Id",
                    errorCode = 404
                });
            }
            return BadRequest(new ErrorMessage()
            {
                errorMessage = "Appointment for past time and days are not allowed ",
                errorCode = 400
            });
        }


        //delete appointment by Id
        [HttpDelete, Route("v1/appointments/{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
        public async Task<ActionResult> DeleteAppointment([FromRoute] Guid appointmentId)
        {
            var isDeleted = await _appointmentBL.DeleteAppointment(appointmentId);
            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound(new ErrorMessage()
            {
                errorMessage = "No Appointment found with the given Id",
                errorCode = 404
            });
        }
    }
}

