using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Professionals.Commands.CreateProfile;

public record CreateProfileCommand(Guid UserId, string BusinessName, string? Specialty, string? Bio) : IRequest<Professional>;
