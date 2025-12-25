using Aesthetic.Domain.Entities;
using MediatR;

namespace Aesthetic.Application.Appointments.Queries.GetCustomerAppointments;

public record GetCustomerAppointmentsQuery(Guid CustomerId) : IRequest<IEnumerable<Appointment>>;
