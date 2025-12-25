using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Appointments.Commands.BookAppointment;

public class BookAppointmentCommandHandler : IRequestHandler<BookAppointmentCommand, Appointment>
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BookAppointmentCommandHandler(
        IAppointmentRepository appointmentRepository,
        IServiceRepository serviceRepository,
        IUnitOfWork unitOfWork)
    {
        _appointmentRepository = appointmentRepository;
        _serviceRepository = serviceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Appointment> Handle(BookAppointmentCommand request, CancellationToken cancellationToken)
    {
        var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
        if (service == null)
        {
            throw new KeyNotFoundException("Service not found.");
        }

        if (!service.IsActive)
        {
            throw new InvalidOperationException("Service is not active.");
        }

        // Calculate EndTime
        var endTime = request.StartTime.AddMinutes(service.DurationMinutes);

        // Check for conflicts
        var hasConflict = await _appointmentRepository.HasConflictAsync(service.ProfessionalId, request.StartTime, endTime);
        if (hasConflict)
        {
            throw new InvalidOperationException("The selected time slot is not available.");
        }
        
        var appointment = new Appointment(
            request.CustomerId,
            service.ProfessionalId,
            request.ServiceId,
            request.StartTime,
            service.DurationMinutes,
            service.Price
        );

        await _appointmentRepository.AddAsync(appointment);
        await _unitOfWork.SaveChangesAsync();

        return appointment;
    }
}
