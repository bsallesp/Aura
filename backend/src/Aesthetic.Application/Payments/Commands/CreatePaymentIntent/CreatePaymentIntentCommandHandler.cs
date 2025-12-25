using Aesthetic.Application.Common.Interfaces.Payments;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Payments.Commands.CreatePaymentIntent;

public class CreatePaymentIntentCommandHandler : IRequestHandler<CreatePaymentIntentCommand, string>
{
    private readonly IPaymentService _paymentService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IProfessionalRepository _professionalRepository;
    private readonly IServiceRepository _serviceRepository;

    public CreatePaymentIntentCommandHandler(
        IPaymentService paymentService,
        IAppointmentRepository appointmentRepository,
        IProfessionalRepository professionalRepository,
        IServiceRepository serviceRepository)
    {
        _paymentService = paymentService;
        _appointmentRepository = appointmentRepository;
        _professionalRepository = professionalRepository;
        _serviceRepository = serviceRepository;
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

        var service = await _serviceRepository.GetByIdAsync(appointment.ServiceId);
        var description = $"Appointment for {service?.Name ?? "Service"}";

        var amount = appointment.PriceAtBooking;
        if (service?.DepositPercentage is not null)
        {
            amount = Math.Round(appointment.PriceAtBooking * service.DepositPercentage.Value, 2);
        }

        return await _paymentService.CreatePaymentIntentAsync(
            amount,
            "usd", // Assuming USD for now, should come from config or service
            description,
            connectedAccountId, 
            0,     // ApplicationFeeAmount
            request.IdempotencyKey,
            metadata
        );
    }
}
