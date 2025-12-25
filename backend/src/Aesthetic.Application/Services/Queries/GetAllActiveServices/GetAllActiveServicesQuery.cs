using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Services.Queries.GetAllActiveServices;

public record GetAllActiveServicesQuery : IRequest<IEnumerable<Service>>;
