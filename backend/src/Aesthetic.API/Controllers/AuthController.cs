using Asp.Versioning;
using Aesthetic.API.Contracts.Authentication;
using Aesthetic.Application.Authentication.Commands.Register;
using Aesthetic.Application.Authentication.Queries.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Aesthetic.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var command = new RegisterCommand(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.Password,
                    request.Role ?? "Client",
                    request.BusinessName);

                var authResult = await _sender.Send(command);

                var response = new AuthenticationResponse(
                    authResult.User.Id,
                    authResult.User.FirstName,
                    authResult.User.LastName,
                    authResult.User.Email,
                    authResult.Token);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var query = new LoginQuery(
                    request.Email,
                    request.Password);

                var authResult = await _sender.Send(query);

                var response = new AuthenticationResponse(
                    authResult.User.Id,
                    authResult.User.FirstName,
                    authResult.User.LastName,
                    authResult.User.Email,
                    authResult.Token);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}