using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Professionals.Commands.UpdateAvailability
{
    public class UpdateAvailabilityCommandHandler : IRequestHandler<UpdateAvailabilityCommand, Unit>
    {
        private readonly IProfessionalRepository _professionalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAvailabilityCommandHandler(IProfessionalRepository professionalRepository, IUnitOfWork unitOfWork)
        {
            _professionalRepository = professionalRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateAvailabilityCommand request, CancellationToken cancellationToken)
        {
            var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId);
            
            if (professional == null)
            {
                throw new KeyNotFoundException($"Professional with ID {request.ProfessionalId} not found.");
            }

            professional.UpdateAvailability(request.DayOfWeek, request.StartTime, request.EndTime, request.IsDayOff);
            
            await _unitOfWork.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
