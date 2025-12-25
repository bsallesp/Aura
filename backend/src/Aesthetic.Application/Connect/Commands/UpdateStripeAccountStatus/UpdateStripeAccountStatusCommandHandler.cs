using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Connect.Commands.UpdateStripeAccountStatus;

public class UpdateStripeAccountStatusCommandHandler : IRequestHandler<UpdateStripeAccountStatusCommand>
{
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateStripeAccountStatusCommandHandler(
        IProfessionalRepository professionalRepository,
        IUnitOfWork unitOfWork)
    {
        _professionalRepository = professionalRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateStripeAccountStatusCommand request, CancellationToken cancellationToken)
    {
        var professional = await _professionalRepository.GetByStripeAccountIdAsync(request.StripeAccountId);
        if (professional == null) return;

        // Check if charges and payouts are enabled
        if (request.ChargesEnabled && request.PayoutsEnabled)
        {
            professional.CompleteStripeOnboarding();
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
