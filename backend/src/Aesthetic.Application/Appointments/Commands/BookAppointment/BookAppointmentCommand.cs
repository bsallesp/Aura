using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Appointments.Commands.BookAppointment;

public record BookAppointmentCommand(Guid CustomerId, Guid ServiceId, DateTime StartTime) : IRequest<Appointment>;
