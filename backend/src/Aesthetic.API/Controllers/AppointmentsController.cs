using Asp.Versioning;
using Aesthetic.API.Contracts.Appointments;
using Aesthetic.Application.Appointments.Commands.BookAppointment;
using Aesthetic.Application.Appointments.Commands.CancelAppointment;
using Aesthetic.Application.Appointments.Commands.ConfirmAppointment;
using Aesthetic.Application.Appointments.Queries.GetAvailableSlots;
using Aesthetic.Application.Appointments.Queries.GetCustomerAppointments;
using Aesthetic.Application.Appointments.Queries.GetProfessionalAppointments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aesthetic.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/appointments")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly ISender _sender;

        public AppointmentsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("slots")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] Guid professionalId, [FromQuery] Guid serviceId, [FromQuery] DateTime date)
        {
            try
            {
                var query = new GetAvailableSlotsQuery(professionalId, serviceId, date);
                var slots = await _sender.Send(query);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(BookAppointmentRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var customerId = Guid.Parse(userIdClaim.Value);

            try
            {
                var command = new BookAppointmentCommand(customerId, request.ServiceId, request.StartTime);
                var appointment = await _sender.Send(command);

                var response = new AppointmentResponse(
                    appointment.Id,
                    appointment.CustomerId,
                    appointment.ProfessionalId,
                    appointment.ServiceId,
                    appointment.StartTime,
                    appointment.EndTime,
                    appointment.PriceAtBooking,
                    appointment.Status.ToString());

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("my-appointments")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            
            var query = new GetCustomerAppointmentsQuery(userId);
            var appointments = await _sender.Send(query);

            var response = appointments.Select(a => new AppointmentResponse(
                a.Id,
                a.CustomerId,
                a.ProfessionalId,
                a.ServiceId,
                a.StartTime,
                a.EndTime,
                a.PriceAtBooking,
                a.Status.ToString()));

            return Ok(response);
        }

        [HttpGet("professional")]
        public async Task<IActionResult> GetProfessionalAppointments()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            
            var query = new GetProfessionalAppointmentsQuery(userId);
            var appointments = await _sender.Send(query);

            var response = appointments.Select(a => new AppointmentResponse(
                a.Id,
                a.CustomerId,
                a.ProfessionalId,
                a.ServiceId,
                a.StartTime,
                a.EndTime,
                a.PriceAtBooking,
                a.Status.ToString()));

            return Ok(response);
        }

        [HttpPut("{appointmentId}/confirm")]
        public async Task<IActionResult> ConfirmAppointment(Guid appointmentId, [FromBody] ConfirmAppointmentRequest request)
        {
            try
            {
                var command = new ConfirmAppointmentCommand(appointmentId, request.PaymentIntentId);
                await _sender.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{appointmentId}/cancel")]
        public async Task<IActionResult> CancelAppointment(Guid appointmentId)
        {
            try
            {
                var command = new CancelAppointmentCommand(appointmentId);
                await _sender.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}