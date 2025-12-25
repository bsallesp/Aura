using MediatR;

namespace Aesthetic.Application.Appointments.Commands.CancelAppointment;

public record CancelAppointmentCommand(Guid AppointmentId, Guid ActorUserId) : IRequest;
