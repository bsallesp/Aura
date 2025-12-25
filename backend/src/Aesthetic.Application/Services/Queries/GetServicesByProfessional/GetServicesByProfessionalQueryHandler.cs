using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Services.Queries.GetServicesByProfessional;

public class GetServicesByProfessionalQueryHandler : IRequestHandler<GetServicesByProfessionalQuery, IEnumerable<Service>>
{
    private readonly IServiceRepository _serviceRepository;

    public GetServicesByProfessionalQueryHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<IEnumerable<Service>> Handle(GetServicesByProfessionalQuery request, CancellationToken cancellationToken)
    {
        return await _serviceRepository.GetByProfessionalIdAsync(request.ProfessionalId);
    }
}
