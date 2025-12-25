using Aesthetic.Domain.Interfaces;
using System;
using MediatR;

namespace Aesthetic.Application.Appointments.Commands.CancelAppointment;

public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelAppointmentCommandHandler(IAppointmentRepository appointmentRepository, IServiceRepository serviceRepository, IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
        if (appointment == null)
        {
            throw new KeyNotFoundException("Appointment not found.");
        }

        var service = await _serviceRepository.GetByIdAsync(appointment.ServiceId);

        decimal? fee = null;
        if (service?.CancelFeePercentage is not null && service.CancelFeeWindowHours is not null)
        {
            var windowStart = appointment.StartTime.AddHours(-service.CancelFeeWindowHours.Value);
            if (DateTime.UtcNow >= windowStart)
            {
                fee = Math.Round(appointment.PriceAtBooking * service.CancelFeePercentage.Value, 2);
            }
        }

        appointment.Cancel(fee);
        await _appointmentRepository.UpdateAsync(appointment);
        await _unitOfWork.SaveChangesAsync();
    }
}
