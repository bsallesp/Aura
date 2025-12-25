using Aesthetic.Application.Appointments.Queries.GetAvailableSlots;
using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Moq;
using Xunit;

namespace Aesthetic.UnitTests.Application.Appointments.Queries
{
    public class GetAvailableSlotsQueryHandlerTests
    {
        private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly GetAvailableSlotsQueryHandler _handler;

        public GetAvailableSlotsQueryHandlerTests()
        {
            _professionalRepositoryMock = new Mock<IProfessionalRepository>();
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _handler = new GetAvailableSlotsQueryHandler(
                _professionalRepositoryMock.Object,
                _serviceRepositoryMock.Object,
                _appointmentRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSlots_WhenAvailabilityExistsAndNoConflicts()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var professional = new Professional(userId, "Test Business", "Test Specialty");
            var professionalId = professional.Id;

            professional.UpdateAvailability(
                DayOfWeek.Thursday,
                new TimeSpan(9, 0, 0), // 09:00
                new TimeSpan(11, 0, 0),  // 11:00
                false
            );

            var service = new Service(professionalId, "Test Service", 100m, 60);
            var serviceId = service.Id;

            var date = new DateTime(2025, 12, 25); // Thursday

            _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId))
                .ReturnsAsync(professional);

            _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId))
                .ReturnsAsync(service);

            _appointmentRepositoryMock.Setup(x => x.GetByProfessionalAndDateAsync(professionalId, date))
                .ReturnsAsync(new List<Appointment>());

            var query = new GetAvailableSlotsQuery(professionalId, serviceId, date);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            // Slots: 09:00-10:00, 09:30-10:30, 10:00-11:00
            Assert.Equal(3, result.Count);
            Assert.Contains(date.Date.AddHours(9), result);
            Assert.Contains(date.Date.AddHours(9).AddMinutes(30), result);
            Assert.Contains(date.Date.AddHours(10), result);
        }

        [Fact]
        public async Task Handle_ShouldExcludeOverlappingSlots_WhenConflictExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var professional = new Professional(userId, "Test Business", "Test Specialty");
            var professionalId = professional.Id;

            professional.UpdateAvailability(
                DayOfWeek.Thursday,
                new TimeSpan(9, 0, 0), // 09:00
                new TimeSpan(11, 0, 0),  // 11:00
                false
            );

            var service = new Service(professionalId, "Test Service", 100m, 60);
            var serviceId = service.Id;
            var date = new DateTime(2025, 12, 25); // Thursday

            var customerId = Guid.NewGuid();
            
            // Appointment at 09:00-10:00
            // Note: Date must be in future relative to system time (2025-12-24)
            var appointment = new Appointment(customerId, professionalId, serviceId, date.Date.AddHours(9), 60, 100m);
            
            var appointments = new List<Appointment> { appointment };

            _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId))
                .ReturnsAsync(professional);

            _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId))
                .ReturnsAsync(service);

            _appointmentRepositoryMock.Setup(x => x.GetByProfessionalAndDateAsync(professionalId, date))
                .ReturnsAsync(appointments);

            var query = new GetAvailableSlotsQuery(professionalId, serviceId, date);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            // 09:00 - Overlap
            // 09:30 - Overlap
            // 10:00 - Free
            Assert.Single(result);
            Assert.Contains(date.Date.AddHours(10), result);
        }
    }
}
