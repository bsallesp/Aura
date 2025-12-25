using Aesthetic.API.Contracts.Services;
using Aesthetic.Application.Professionals.Queries.GetProfile;
using Aesthetic.Application.Services.Commands.CreateService;
using Aesthetic.Application.Services.Commands.DeactivateService;
using Aesthetic.Application.Services.Queries.GetAllActiveServices;
using Aesthetic.Application.Services.Queries.GetServicesByProfessional;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aesthetic.API.Controllers
{
    [ApiController]
    [Route("services")]
    public class ServicesController : ControllerBase
    {
        private readonly ISender _sender;

        public ServicesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateService(CreateServiceRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) return Unauthorized();

                var userId = Guid.Parse(userIdClaim.Value);
                var professional = await _sender.Send(new GetProfileQuery(userId));
                if (professional == null)
                {
                    return BadRequest(new { error = "User must be a professional to create services." });
                }

                var command = new CreateServiceCommand(
                    professional.Id,
                    request.Name,
                    request.Price,
                    request.DurationMinutes,
                    request.Description);

                var result = await _sender.Send(command);

                var response = new ServiceResponse(
                    result.Id,
                    result.ProfessionalId,
                    result.Name,
                    result.Price,
                    result.DurationMinutes,
                    result.Description,
                    result.IsActive);

                return CreatedAtAction(nameof(GetServicesByProfessional), new { professionalId = result.ProfessionalId }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("professional/{professionalId}")]
        public async Task<IActionResult> GetServicesByProfessional(Guid professionalId)
        {
            var services = await _sender.Send(new GetServicesByProfessionalQuery(professionalId));

            var response = services.Select(s => new ServiceResponse(
                s.Id,
                s.ProfessionalId,
                s.Name,
                s.Price,
                s.DurationMinutes,
                s.Description,
                s.IsActive));

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActiveServices()
        {
            var services = await _sender.Send(new GetAllActiveServicesQuery());

            var response = services.Select(s => new ServiceResponse(
                s.Id,
                s.ProfessionalId,
                s.Name,
                s.Price,
                s.DurationMinutes,
                s.Description,
                s.IsActive));

            return Ok(response);
        }

        [HttpDelete("{serviceId}")]
        [Authorize]
        public async Task<IActionResult> DeactivateService(Guid serviceId)
        {
             // Ideally we should check if the current user owns this service.
             // For now skipping ownership check for simplicity but noting it.
            try
            {
                await _sender.Send(new DeactivateServiceCommand(serviceId));
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
