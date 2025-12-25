using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Services.Commands.CreateService;

public record CreateServiceCommand(Guid ProfessionalId, string Name, decimal Price, int DurationMinutes, string? Description) : IRequest<Service>;
