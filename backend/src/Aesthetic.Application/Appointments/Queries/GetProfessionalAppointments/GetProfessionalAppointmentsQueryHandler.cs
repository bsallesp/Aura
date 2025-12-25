using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Appointments.Queries.GetProfessionalAppointments;

public class GetProfessionalAppointmentsQueryHandler : IRequestHandler<GetProfessionalAppointmentsQuery, IEnumerable<Appointment>>
{
    private readonly IAppointmentRepository _appointmentRepository;

    public GetProfessionalAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<IEnumerable<Appointment>> Handle(GetProfessionalAppointmentsQuery request, CancellationToken cancellationToken)
    {
        return await _appointmentRepository.GetByProfessionalIdAsync(request.ProfessionalId);
    }
}
