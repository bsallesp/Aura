using MediatR;

namespace Aesthetic.Application.Connect.Commands.StartOnboarding;

public record StartOnboardingCommand(Guid UserId, string FrontendUrl) : IRequest<string>;
