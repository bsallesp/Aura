using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Aesthetic.Application.Common.Interfaces.Security;
using MediatR;

namespace Aesthetic.Application.Appointments.Queries.GetProfessionalAppointments;

public class GetProfessionalAppointmentsQueryHandler : IRequestHandler<GetProfessionalAppointmentsQuery, IEnumerable<Appointment>>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAuditService _auditService;

    public GetProfessionalAppointmentsQueryHandler(IAppointmentRepository appointmentRepository, IAuditService auditService)
    {
        _appointmentRepository = appointmentRepository;
        _auditService = auditService;
    }

    public async Task<IEnumerable<Appointment>> Handle(GetProfessionalAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _appointmentRepository.GetByProfessionalIdAsync(request.ProfessionalId);
        
        // Log access to professional appointments
        await _auditService.LogAsync(request.ActorUserId, "Appointment.List.Read", "Professional", request.ProfessionalId, "{\"Type\":\"Professional\"}");

        return appointments;
    }
}
