using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found.");
        }

        appointment.Cancel();
        await _appointmentRepository.UpdateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();
    }
}
