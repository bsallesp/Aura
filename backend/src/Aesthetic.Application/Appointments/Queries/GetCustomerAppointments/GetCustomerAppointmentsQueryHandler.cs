using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Aesthetic.Application.Common.Interfaces.Security;
using MediatR;

namespace Aesthetic.Application.Appointments.Queries.GetCustomerAppointments;

public class GetCustomerAppointmentsQueryHandler : IRequestHandler<GetCustomerAppointmentsQuery, IEnumerable<Appointment>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAuditService _auditService;

    public GetCustomerAppointmentsQueryHandler(IAppointmentRepository appointmentRepository, IAuditService auditService)
    {
        _appointmentRepository = appointmentRepository;
        _auditService = auditService;
    }

    public async Task<IEnumerable<Appointment>> Handle(GetCustomerAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _appointmentRepository.GetByCustomerIdAsync(request.CustomerId);
        
        // Log access to own appointments
        await _auditService.LogAsync(request.ActorUserId, "Appointment.List.Read", "User", request.CustomerId, "{\"Type\":\"Customer\"}");
        
        return appointments;
    }
}
