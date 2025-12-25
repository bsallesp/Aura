using Aesthetic.Application.Appointments.Commands.BookAppointment;
using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Moq;
using Xunit;

namespace Aesthetic.UnitTests.Application.Appointments.Commands
{
    public class BookAppointmentCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly BookAppointmentCommandHandler _handler;

        public BookAppointmentCommandHandlerTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _professionalRepositoryMock = new Mock<IProfessionalRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new BookAppointmentCommandHandler(
                _appointmentRepositoryMock.Object,
                _serviceRepositoryMock.Object,
                _professionalRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProfessionalAvailabilityIsMissing()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();
            var professionalId = Guid.NewGuid();
            var startTime = new DateTime(2025, 12, 25, 10, 0, 0); // Thursday

            var service = new Service(professionalId, "Test Service", 100, 60);
            var professional = new Professional(Guid.NewGuid(), "Test Biz", "Spec");

            // Professional has NO availability set
            _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId)).ReturnsAsync(service);
            _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId)).ReturnsAsync(professional);

            var command = new BookAppointmentCommand(customerId, serviceId, startTime);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenTimeIsOutsideWorkingHours()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();
            var professionalId = Guid.NewGuid();
            var startTime = new DateTime(2025, 12, 25, 20, 0, 0); // Thursday 20:00 (8 PM)

            var service = new Service(professionalId, "Test Service", 100, 60);
            var professional = new Professional(Guid.NewGuid(), "Test Biz", "Spec");
            professional.UpdateAvailability(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0), false);

            _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId)).ReturnsAsync(service);
            _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId)).ReturnsAsync(professional);

            var command = new BookAppointmentCommand(customerId, serviceId, startTime);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("The selected time is outside of professional's working hours.", ex.Message);
        }

        [Fact]
        public async Task Handle_ShouldSucceed_WhenSlotIsValid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();
            var professionalId = Guid.NewGuid();
            var startTime = new DateTime(2025, 12, 25, 10, 0, 0); // Thursday 10:00

            var service = new Service(professionalId, "Test Service", 100, 60);
            var professional = new Professional(Guid.NewGuid(), "Test Biz", "Spec");
            professional.UpdateAvailability(DayOfWeek.Thursday, new TimeSpan(9, 0, 0), new TimeSpan(17, 0, 0), false);

            _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId)).ReturnsAsync(service);
            _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId)).ReturnsAsync(professional);
            _appointmentRepositoryMock.Setup(x => x.HasConflictAsync(professionalId, startTime, startTime.AddMinutes(60)))
                .ReturnsAsync(false);

            var command = new BookAppointmentCommand(customerId, serviceId, startTime);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerId, result.CustomerId);
            Assert.Equal(professionalId, result.ProfessionalId);
            _appointmentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Appointment>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
