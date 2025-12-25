using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Appointments.Queries.GetProfessionalAppointments;

public record GetProfessionalAppointmentsQuery(Guid ProfessionalId, Guid ActorUserId) : IRequest<IEnumerable<Appointment>>;
