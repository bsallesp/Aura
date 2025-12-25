using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Professionals.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Professional?>
{
    private readonly IProfessionalRepository _professionalRepository;

    public GetProfileQueryHandler(IProfessionalRepository professionalRepository)
    {
        _professionalRepository = professionalRepository;
    }

    public async Task<Professional?> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await _professionalRepository.GetByUserIdAsync(request.UserId);
        return result;
    }
}
