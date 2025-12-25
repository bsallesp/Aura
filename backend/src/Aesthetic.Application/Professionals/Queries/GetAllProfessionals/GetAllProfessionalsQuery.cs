using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Professionals.Queries.GetAllProfessionals;

public record GetAllProfessionalsQuery : IRequest<IEnumerable<Professional>>;
