using Aesthetic.Application.Appointments.Commands.BookAppointment;
using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Moq;

namespace Aesthetic.UnitTests.Application.Appointments.Commands
{
    public class BookAppointmentCommandHandlerTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly BookAppointmentCommandHandler _handler;

        public BookAppointmentCommandHandlerTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new BookAppointmentCommandHandler(
                _appointmentRepositoryMock.Object,
                _serviceRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenTimeSlotIsNotAvailable()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();
            var professionalId = Guid.NewGuid();
            var startTime = DateTime.UtcNow.AddDays(1);
            
            var service = new Service(professionalId, "Test Service", 100, 60);

            _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId))
                .ReturnsAsync(service);

            _appointmentRepositoryMock.Setup(x => x.HasConflictAsync(professionalId, startTime, It.IsAny<DateTime>()))
                .ReturnsAsync(true);

            var command = new BookAppointmentCommand(customerId, serviceId, startTime);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldSucceed_WhenTimeSlotIsAvailable()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();
            var professionalId = Guid.NewGuid();
            var startTime = DateTime.UtcNow.AddDays(1);
            
            var service = new Service(professionalId, "Test Service", 100, 60);

            _serviceRepositoryMock.Setup(x => x.GetByIdAsync(serviceId))
                .ReturnsAsync(service);

            _appointmentRepositoryMock.Setup(x => x.HasConflictAsync(professionalId, startTime, It.IsAny<DateTime>()))
                .ReturnsAsync(false);

            var command = new BookAppointmentCommand(customerId, serviceId, startTime);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerId, result.CustomerId);
            Assert.Equal(serviceId, result.ServiceId);
            Assert.Equal(professionalId, result.ProfessionalId);
            Assert.Equal(startTime, result.StartTime);
            
            _appointmentRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Appointment>()), Times.Once());
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
