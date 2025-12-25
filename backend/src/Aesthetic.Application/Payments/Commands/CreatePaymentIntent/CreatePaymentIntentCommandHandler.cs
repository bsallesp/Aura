using Aesthetic.Application.Common.Interfaces.Payments;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Payments.Commands.CreatePaymentIntent;

public class CreatePaymentIntentCommandHandler : IRequestHandler<CreatePaymentIntentCommand, string>
{
    private readonly IPaymentService _paymentService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IProfessionalRepository _professionalRepository;

    public CreatePaymentIntentCommandHandler(
        IPaymentService paymentService,
        IAppointmentRepository appointmentRepository,
        IProfessionalRepository professionalRepository)
    {
        _paymentService = paymentService;
        _appointmentRepository = appointmentRepository;
        _professionalRepository = professionalRepository;
    }

    public async Task<string> Handle(CreatePaymentIntentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found.");
        }

        if (appointment.Status != Domain.Enums.AppointmentStatus.Pending)
        {
            throw new InvalidOperationException("Appointment is not in pending status.");
        }

        // Fetch Professional to get StripeAccountId (for transfers)
        // Note: In a real scenario, we might use Destination Charges or Separate Charges and Transfers.
        // For now, we'll keep it simple or assume platform payments if StripeAccountId is not set.
        var professional = await _professionalRepository.GetByIdAsync(appointment.ProfessionalId);
        string? connectedAccountId = professional?.StripeAccountId;

        var metadata = new Dictionary<string, string>
        {
            { "AppointmentId", appointment.Id.ToString() },
            { "CustomerId", appointment.CustomerId.ToString() },
            { "ProfessionalId", appointment.ProfessionalId.ToString() }
        };

        var description = $"Appointment for {appointment.Service?.Name ?? "Service"}";

        return await _paymentService.CreatePaymentIntentAsync(
            appointment.PriceAtBooking,
            "usd", // Assuming USD for now, should come from config or service
            description,
            connectedAccountId, 
            0,     // ApplicationFeeAmount
            request.IdempotencyKey,
            metadata
        );
    }
}
