using MediatR;

namespace Aesthetic.Application.Connect.Commands.UpdateStripeAccountStatus;

public record UpdateStripeAccountStatusCommand(string StripeAccountId, bool ChargesEnabled, bool PayoutsEnabled) : IRequest;
