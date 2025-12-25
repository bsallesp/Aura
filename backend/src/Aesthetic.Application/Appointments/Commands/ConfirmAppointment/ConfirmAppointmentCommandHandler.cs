using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Appointments.Commands.ConfirmAppointment;

public class ConfirmAppointmentCommandHandler : IRequestHandler<ConfirmAppointmentCommand>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found.");
        }

        appointment.Confirm(request.PaymentIntentId);
        await _appointmentRepository.UpdateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();
    }
}
