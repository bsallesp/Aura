using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Professionals.Queries.GetAllProfessionals;

public class GetAllProfessionalsQueryHandler : IRequestHandler<GetAllProfessionalsQuery, IEnumerable<Professional>>
{
    private readonly IProfessionalRepository _professionalRepository;

    public GetAllProfessionalsQueryHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository;
    }

    public async Task<IEnumerable<Professional>> Handle(GetAllProfessionalsQuery request, CancellationToken cancellationToken)
    {
        return await _professionalRepository.GetAllAsync();
    }
}
