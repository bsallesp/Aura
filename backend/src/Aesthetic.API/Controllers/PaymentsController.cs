using Asp.Versioning;
using Aesthetic.API.Contracts.Payments;
using Aesthetic.Application.Payments.Commands.CreatePaymentIntent;
using Aesthetic.Application.Appointments.Commands.ConfirmAppointment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Aesthetic.Infrastructure.Payments;

namespace Aesthetic.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly StripeSettings _stripeSettings;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(ISender sender, IOptions<StripeSettings> stripeSettings, ILogger<PaymentsController> logger)
        {
            _sender = sender;
            _stripeSettings = stripeSettings.Value;
            _logger = logger;
        }

        [Authorize]
        [HttpPost("create-intent")]
        [Aesthetic.API.Filters.IdempotencyKeyRequired]
        public async Task<IActionResult> CreatePaymentIntent(CreatePaymentIntentRequest request)
        {
            try
            {
                var idempotencyKey = Request.Headers["Idempotency-Key"].ToString();
                
                // Now we expect request.AppointmentId from the client, or we could infer from context if we change request DTO.
                // Assuming CreatePaymentIntentRequest has been updated or we reuse Amount field for AppointmentId (bad practice).
                // Let's check CreatePaymentIntentRequest contract.
                
                // Correction: The contract needs to be updated. 
                // But since I can't see the contract right now, I'll assume I need to change the usage here.
                // Wait, I saw the contract earlier, it had Amount/Currency/Description.
                // I need to update the contract first or pass AppointmentId here.
                
                // Let's assume for this step I will update the Request object in a separate tool call if needed.
                // Actually I should update the contract file.
                
                // Since I'm editing the controller, let's write the code assuming the contract has AppointmentId.
                // I will update the contract in the next step.
                
                var command = new CreatePaymentIntentCommand(
                    request.AppointmentId,
                    string.IsNullOrEmpty(idempotencyKey) ? null : idempotencyKey);

                var clientSecret = await _sender.Send(command);

                var response = new CreatePaymentIntentResponse(clientSecret);

                return Ok(response);
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
            
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _stripeSettings.WebhookSecret
                );

                if (stripeEvent.Type == "payment_intent.succeeded")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    
                    if (paymentIntent != null && paymentIntent.Metadata.ContainsKey("AppointmentId"))
                    {
                        var appointmentId = Guid.Parse(paymentIntent.Metadata["AppointmentId"]);
                        
                        var command = new ConfirmAppointmentCommand(appointmentId, paymentIntent.Id);
                        await _sender.Send(command);
                        
                        _logger.LogInformation("Appointment {AppointmentId} confirmed via webhook.", appointmentId);
                    }
                }
                else
                {
                    _logger.LogInformation("Unhandled event type: {EventType}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogError(e, "Stripe Webhook Error");
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Webhook Processing Error");
                return StatusCode(500);
            }
        }
    }
}
