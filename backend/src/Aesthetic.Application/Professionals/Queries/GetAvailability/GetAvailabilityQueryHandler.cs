using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Professionals.Queries.GetAvailability
{
    public class GetAvailabilityQueryHandler : IRequestHandler<GetAvailabilityQuery, List<ProfessionalAvailability>>
    {
        private readonly IProfessionalRepository _professionalRepository;

        public GetAvailabilityQueryHandler(IProfessionalRepository professionalRepository)
        {
            _professionalRepository = professionalRepository;
        }

        public async Task<List<ProfessionalAvailability>> Handle(GetAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId);

            if (professional == null)
            {
                throw new KeyNotFoundException($"Professional with ID {request.ProfessionalId} not found.");
            }

            return professional.Availabilities.ToList();
        }
    }
}
