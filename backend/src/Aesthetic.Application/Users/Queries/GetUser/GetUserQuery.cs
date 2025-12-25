using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Users.Queries.GetUser;

public record GetUserQuery(Guid UserId) : IRequest<User?>;
