using Aesthetic.Domain.Interfaces;
using MediatR;

namespace Aesthetic.Application.Appointments.Queries.GetAvailableSlots
{
    public class GetAvailableSlotsQueryHandler : IRequestHandler<GetAvailableSlotsQuery, List<DateTime>>
    {
        private readonly IProfessionalRepository _professionalRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public GetAvailableSlotsQueryHandler(
            IProfessionalRepository professionalRepository,
            IServiceRepository serviceRepository,
            IAppointmentRepository appointmentRepository)
        {
            _professionalRepository = professionalRepository;
            _serviceRepository = serviceRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<DateTime>> Handle(GetAvailableSlotsQuery request, CancellationToken cancellationToken)
        {
            // 1. Get Professional Availability
            var professional = await _professionalRepository.GetByIdAsync(request.ProfessionalId);
            if (professional == null) throw new KeyNotFoundException("Professional not found.");

            var dayOfWeek = request.Date.DayOfWeek;
            var availability = professional.Availabilities.FirstOrDefault(a => a.DayOfWeek == dayOfWeek);

            // If no availability set or is day off, return empty
            if (availability == null || availability.IsDayOff)
            {
                return new List<DateTime>();
            }

            // 2. Get Service Duration
            var service = await _serviceRepository.GetByIdAsync(request.ServiceId);
            if (service == null) throw new KeyNotFoundException("Service not found.");
            
            // 3. Get Existing Appointments
            // Note: We need a method in repo to get appointments by date/professional
            // For now assuming we can fetch all and filter in memory or add method to repo.
            // Let's assume we add GetByDateAsync to IAppointmentRepository
            var appointments = await _appointmentRepository.GetByProfessionalAndDateAsync(request.ProfessionalId, request.Date);

            // 4. Calculate Slots
            var slots = new List<DateTime>();
            var currentTime = request.Date.Date.Add(availability.StartTime);
            var endTime = request.Date.Date.Add(availability.EndTime);

            while (currentTime.AddMinutes(service.DurationMinutes) <= endTime)
            {
                var slotEnd = currentTime.AddMinutes(service.DurationMinutes);

                // Check collision
                bool isOverlapping = appointments.Any(a => 
                    (a.StartTime < slotEnd && a.EndTime > currentTime) &&
                    a.Status != Aesthetic.Domain.Enums.AppointmentStatus.Cancelled
                );

                if (!isOverlapping)
                {
                    slots.Add(currentTime);
                }

                // Step: 30 mins or Service Duration? 
                // Usually slots are fixed intervals (e.g. every 30 mins or 15 mins)
                // Let's assume 30 mins step for now to allow flexibility, or service duration.
                // Best practice: Configurable interval. Let's use 30 mins.
                currentTime = currentTime.AddMinutes(30); 
            }

            return slots;
        }
    }
}
