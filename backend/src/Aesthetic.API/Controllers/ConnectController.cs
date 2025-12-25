using Asp.Versioning;
using Aesthetic.Application.Connect.Commands.StartOnboarding;
using Aesthetic.Application.Connect.Commands.UpdateStripeAccountStatus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aesthetic.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/connect")]
    [Authorize]
    public class ConnectController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IConfiguration _configuration;

        public ConnectController(
            ISender sender,
            IConfiguration configuration)
        {
            _sender = sender;
            _configuration = configuration;
        }

        [HttpPost("onboarding")]
        public async Task<IActionResult> StartOnboarding()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var userId = Guid.Parse(userIdClaim.Value);
            
            var frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000";

            try
            {
                var accountLinkUrl = await _sender.Send(new StartOnboardingCommand(userId, frontendUrl));
                return Ok(new { url = accountLinkUrl });
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("webhook")]
        [AllowAnonymous]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];
            var webhookSecret = _configuration["Stripe:WebhookSecret"]; // Ensure this is set in appsettings.json

            try
            {
                var stripeEvent = Stripe.EventUtility.ConstructEvent(
                    json,
                    stripeSignature,
                    webhookSecret
                );

                if (stripeEvent.Type == "account.updated")
                {
                    if (stripeEvent.Data.Object is Stripe.Account account)
                    {
                        await _sender.Send(new UpdateStripeAccountStatusCommand(
                            account.Id,
                            account.ChargesEnabled,
                            account.PayoutsEnabled));
                    }
                }

                return Ok();
            }
            catch (Stripe.StripeException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                // Log exception
                return StatusCode(500, ex.Message);
            }
        }
    }
}
