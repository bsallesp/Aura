using MediatR;

namespace Aesthetic.Application.Services.Commands.DeactivateService;

public record DeactivateServiceCommand(Guid ServiceId, Guid ActorUserId) : IRequest;
