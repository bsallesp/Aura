using MediatR;

namespace Aesthetic.Application.Appointments.Commands.ConfirmAppointment;

public record ConfirmAppointmentCommand(Guid AppointmentId, string PaymentIntentId) : IRequest;
