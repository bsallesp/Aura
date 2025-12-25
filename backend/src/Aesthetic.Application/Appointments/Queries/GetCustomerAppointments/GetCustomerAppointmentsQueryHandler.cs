using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Appointments.Queries.GetCustomerAppointments;

public class GetCustomerAppointmentsQueryHandler : IRequestHandler<GetCustomerAppointmentsQuery, IEnumerable<Appointment>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public GetCustomerAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<IEnumerable<Appointment>> Handle(GetCustomerAppointmentsQuery request, CancellationToken cancellationToken)
    {
        return await _appointmentRepository.GetByCustomerIdAsync(request.CustomerId);
    }
}
