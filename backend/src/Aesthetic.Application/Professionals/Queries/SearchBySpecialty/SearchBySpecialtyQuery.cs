using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Professionals.Queries.SearchBySpecialty;

public record SearchBySpecialtyQuery(string Specialty) : IRequest<IEnumerable<Professional>>;
