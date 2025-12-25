using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Professionals.Queries.GetAvailability
{
    public record GetAvailabilityQuery(Guid ProfessionalId) : IRequest<List<ProfessionalAvailability>>;
}
