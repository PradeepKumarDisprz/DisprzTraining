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

        ErrorDetails badRequest = new ErrorDetails()
        {
            errorMessage = "Check the Date and Time",
            errorCode = 400
        };
        ErrorDetails notFound = new ErrorDetails()
        {
            errorMessage = "No Appointment found with specified Id",
            errorCode = 404
        };
        ErrorDetails conflict = new ErrorDetails()
        {
            errorMessage = "There is another Meeting with Same Time",
            errorCode = 409
        };


        //getAllAppointments
        [HttpGet, Route("v1/appointments/fetch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentBL.GetAppointments();
            return Ok(appointments);
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
            return Ok(existingAppointmentById);
        }


        //add new appointment
        [HttpPost, Route("v1/appointments")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(NewAppointmentId))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult> AddNewAppointment([FromBody] AppointmentDetail newAppointment)
        {
          if(await _appointmentBL.CheckDateAndTimeFormat(newAppointment))
          {
                var newAppointmentId = await _appointmentBL.AddNewAppointment(newAppointment);
                if (newAppointmentId!=null)
                {
                    var uri = $"v1/appointments/{newAppointmentId}";
                    return Created(uri, newAppointmentId);    
                    // Created(nameof(GetappointmentById),newAppointmentId);
                }
                else
                {
                 return Conflict(new ErrorDetails()
                 {
                    errorMessage="exception",
                    errorCode=409
                 });   
                }
          }
            return BadRequest();
        }

        //update existing appointment
        [HttpPut, Route("v1/appointments/{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult> UpdateExistingAppointment([FromRoute] Guid appointmentId, [FromBody] AppointmentDetail updateAppointment)
        {
            
               if(await _appointmentBL.CheckDateAndTimeFormat(updateAppointment))
               {
                var isAnyAppointment = await _appointmentBL.GetAppointmentById(appointmentId);
                if (isAnyAppointment.Any())
                {
                    bool isConflict = await _appointmentBL.UpdateExistingAppointment(appointmentId, updateAppointment);
                    if (isConflict)
                    {
                        return Conflict(conflict);
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                return NotFound(notFound);
               }
                return BadRequest(badRequest);
           
        }

        //change conflict function

        //delete appointment by Id
        [HttpDelete, Route("v1/appointments/{appointmentId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]
        public async Task<ActionResult> DeleteAppointment([FromRoute] Guid appointmentId)
        {
            var isDeleted = await _appointmentBL.DeleteAppointment(appointmentId);
            if (isDeleted)
            {
                return NoContent();
            }
            else
                return NotFound(notFound);
        }
    }
}

