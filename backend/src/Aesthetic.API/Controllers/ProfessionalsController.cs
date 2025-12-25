using Asp.Versioning;
using Aesthetic.API.Contracts.Professionals;
using Aesthetic.Application.Professionals.Commands.CreateProfile;
using Aesthetic.Application.Professionals.Commands.UpdateAvailability;
using Aesthetic.Application.Professionals.Queries.GetAllProfessionals;
using Aesthetic.Application.Professionals.Queries.GetAvailability;
using Aesthetic.Application.Professionals.Queries.GetProfile;
using Aesthetic.Application.Professionals.Queries.SearchBySpecialty;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aesthetic.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/professionals")]
    public class ProfessionalsController : ControllerBase
    {
        private readonly ISender _sender;

        public ProfessionalsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProfile(CreateProfessionalRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);

            try
            {
                var command = new CreateProfileCommand(
                    userId,
                    request.BusinessName,
                    request.Specialty,
                    request.Bio);

                var professional = await _sender.Send(command);

                var response = new ProfessionalResponse(
                    professional.UserId,
                    professional.BusinessName,
                    professional.Specialty,
                    professional.Bio);

                return CreatedAtAction(nameof(GetProfile), new { userId = professional.UserId }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            var query = new GetProfileQuery(userId);
            var professional = await _sender.Send(query);
            
            if (professional == null) return NotFound();

            var response = new ProfessionalResponse(
                professional.UserId,
                professional.BusinessName,
                professional.Specialty,
                professional.Bio);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? specialty)
        {
            IEnumerable<Aesthetic.Domain.Entities.Professional> professionals;

            if (string.IsNullOrWhiteSpace(specialty))
            {
                professionals = await _sender.Send(new GetAllProfessionalsQuery());
            }
            else
            {
                professionals = await _sender.Send(new SearchBySpecialtyQuery(specialty));
            }

            var response = professionals.Select(p => new ProfessionalResponse(
                p.UserId,
                p.BusinessName,
                p.Specialty,
                p.Bio));

            return Ok(response);
        }

        [HttpPut("availability")]
        [Authorize]
        public async Task<IActionResult> UpdateAvailability(UpdateAvailabilityRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            var professional = await _sender.Send(new GetProfileQuery(userId));
            
            if (professional == null) return NotFound("Professional profile not found.");

            var command = new UpdateAvailabilityCommand(
                professional.Id,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                request.IsDayOff
            );

            await _sender.Send(command);

            return NoContent();
        }

        [HttpGet("{userId}/availability")]
        public async Task<IActionResult> GetAvailability(Guid userId)
        {
            var professional = await _sender.Send(new GetProfileQuery(userId));
            if (professional == null) return NotFound("Professional not found.");

            var query = new GetAvailabilityQuery(professional.Id);
            var availabilities = await _sender.Send(query);

            var response = availabilities.Select(a => new AvailabilityResponse(
                a.DayOfWeek,
                a.StartTime,
                a.EndTime,
                a.IsDayOff
            ));

            return Ok(response);
        }
    }
}