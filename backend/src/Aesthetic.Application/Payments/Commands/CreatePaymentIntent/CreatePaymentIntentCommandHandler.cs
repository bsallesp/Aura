using Aesthetic.Application.Common.Interfaces.Payments;
using MediatR;

namespace Aesthetic.Application.Payments.Commands.CreatePaymentIntent;

public class CreatePaymentIntentCommandHandler : IRequestHandler<CreatePaymentIntentCommand, string>
{
    private readonly IPaymentService _paymentService;

    public CreatePaymentIntentCommandHandler(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    public async Task<string> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
    {
        return await _paymentService.CreatePaymentIntentAsync(
            request.Amount,
            request.Currency,
            request.Description ?? string.Empty,
            null, // CustomerId - currently null in controller
            0,     // ApplicationFeeAmount - currently 0 in controller
            request.IdempotencyKey
        );
    }
}
