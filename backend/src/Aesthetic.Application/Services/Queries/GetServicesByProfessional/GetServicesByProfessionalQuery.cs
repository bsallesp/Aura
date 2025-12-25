using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Services.Queries.GetServicesByProfessional;

public record GetServicesByProfessionalQuery(Guid ProfessionalId) : IRequest<IEnumerable<Service>>;
