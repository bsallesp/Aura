using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Appointments.Commands.BookAppointment;

public class BookAppointmentCommandHandler : IRequestHandler<BookAppointmentCommand, Appointment>
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IProfessionalRepository _professionalRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BookAppointmentCommandHandler(
            IAppointmentRepository appointmentRepository,
            IServiceRepository serviceRepository,
            IProfessionalRepository professionalRepository,
            IUnitOfWork unitOfWork)
        {
            _appointmentRepository = appointmentRepository;
            _serviceRepository = serviceRepository;
            _professionalRepository = professionalRepository;
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

            // Check Professional Availability
            var professional = await _professionalRepository.GetByIdAsync(service.ProfessionalId);
            if (professional == null) throw new KeyNotFoundException("Professional not found.");

            var dayOfWeek = request.StartTime.DayOfWeek;
            var availability = professional.Availabilities.FirstOrDefault(a => a.DayOfWeek == dayOfWeek);

            if (availability == null || availability.IsDayOff)
            {
                throw new InvalidOperationException("Professional is not available on this day.");
            }

            var timeOfDayStart = request.StartTime.TimeOfDay;
            var timeOfDayEnd = endTime.TimeOfDay;

            // Handle overnight shifts if necessary, but assuming standard day shifts for now
            if (timeOfDayStart < availability.StartTime || timeOfDayEnd > availability.EndTime)
            {
                throw new InvalidOperationException("The selected time is outside of professional's working hours.");
            }

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
