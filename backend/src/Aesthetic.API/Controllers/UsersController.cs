using Aesthetic.API.Contracts.Users;
using Aesthetic.Application.Users.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aesthetic.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);

            var user = await _sender.Send(new GetUserQuery(userId));
            if (user == null) return NotFound();

            var response = new UserResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email);

            return Ok(response);
        }
    }
}
