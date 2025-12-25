using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Professionals.Queries.GetProfile;

public record GetProfileQuery(Guid UserId) : IRequest<Professional?>;
