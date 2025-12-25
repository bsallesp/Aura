using Aesthetic.Application.Common.Interfaces.Payments;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Connect.Commands.StartOnboarding;

public class StartOnboardingCommandHandler : IRequestHandler<StartOnboardingCommand, string>
{
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IPaymentService _paymentService;
    private readonly IUnitOfWork _unitOfWork;

    public StartOnboardingCommandHandler(
        IProfessionalRepository professionalRepository,
        IPaymentService paymentService,
        IUnitOfWork unitOfWork)
    {
        _professionalRepository = professionalRepository;
        _paymentService = paymentService;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> Handle(StartOnboardingCommand request, CancellationToken cancellationToken)
    {
        var professional = await _professionalRepository.GetByUserIdAsync(request.UserId);

        if (professional == null)
        {
            throw new KeyNotFoundException("User is not registered as a professional.");
        }

        // Create Stripe Account if not exists
        if (string.IsNullOrEmpty(professional.StripeAccountId))
        {
            var email = professional.User?.Email ?? professional.BusinessName + "@example.com"; // Fallback if user not loaded or no email
            var accountId = await _paymentService.CreateConnectedAccountAsync(email);
            
            professional.UpdateStripeAccountId(accountId);
            await _unitOfWork.SaveChangesAsync();
        }

        // Generate Account Link
        var returnUrl = $"{request.FrontendUrl}/professional/onboarding/success";
        var refreshUrl = $"{request.FrontendUrl}/professional/onboarding/refresh";

        var accountLinkUrl = await _paymentService.CreateAccountLinkAsync(
            professional.StripeAccountId!,
            refreshUrl,
            returnUrl);

        return accountLinkUrl;
    }
}
