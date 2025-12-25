using MediatR;

namespace Aesthetic.Application.Payments.Commands.CreatePaymentIntent;

public record CreatePaymentIntentCommand(
    decimal Amount,
    string Currency,
    string? Description
) : IRequest<string>;
