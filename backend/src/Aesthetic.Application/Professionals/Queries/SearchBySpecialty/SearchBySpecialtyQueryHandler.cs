using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Professionals.Queries.SearchBySpecialty;

public class SearchBySpecialtyQueryHandler : IRequestHandler<SearchBySpecialtyQuery, IEnumerable<Professional>>
{
    private readonly IProfessionalRepository _professionalRepository;

    public SearchBySpecialtyQueryHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository;
    }

    public async Task<IEnumerable<Professional>> Handle(SearchBySpecialtyQuery request, CancellationToken cancellationToken)
    {
        return await _professionalRepository.GetBySpecialtyAsync(request.Specialty);
    }
}
