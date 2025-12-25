using Aesthetic.API.Contracts.Payments;
using Aesthetic.Application.Payments.Commands.CreatePaymentIntent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aesthetic.API.Controllers
{
    [ApiController]
    [Route("payments")]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly ISender _sender;

        public PaymentsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("create-intent")]
        public async Task<IActionResult> CreatePaymentIntent(CreatePaymentIntentRequest request)
        {
            try
            {
                var command = new CreatePaymentIntentCommand(
                    request.Amount,
                    request.Currency,
                    request.Description);

                var clientSecret = await _sender.Send(command);

                var response = new CreatePaymentIntentResponse(clientSecret);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
