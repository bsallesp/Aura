using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Services.Queries.GetAllActiveServices;

public class GetAllActiveServicesQueryHandler : IRequestHandler<GetAllActiveServicesQuery, IEnumerable<Service>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetAllActiveServicesQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<IEnumerable<Service>> Handle(GetAllActiveServicesQuery request, CancellationToken cancellationToken)
    {
        return await _serviceRepository.GetActiveServicesAsync();
    }
}
