using MediatR;

namespace Aesthetic.Application.Services.Commands.UpdateServicePolicies;

public record UpdateServicePoliciesCommand(Guid ServiceId, decimal? DepositPercentage, decimal? CancelFeePercentage, int? CancelFeeWindowHours, Guid ActorUserId) : IRequest;
