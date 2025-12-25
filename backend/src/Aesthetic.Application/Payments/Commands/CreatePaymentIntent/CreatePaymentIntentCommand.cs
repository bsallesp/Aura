using MediatR;

namespace Aesthetic.Application.Payments.Commands.CreatePaymentIntent;

public record CreatePaymentIntentCommand(Guid AppointmentId, string? IdempotencyKey) : IRequest<string>;
