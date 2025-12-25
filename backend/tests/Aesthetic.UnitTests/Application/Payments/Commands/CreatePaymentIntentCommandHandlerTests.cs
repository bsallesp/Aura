using Aesthetic.Application.Common.Interfaces.Payments;
using Aesthetic.Application.Payments.Commands.CreatePaymentIntent;
using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Moq;
using Xunit;

namespace Aesthetic.UnitTests.Application.Payments.Commands
{
    public class CreatePaymentIntentCommandHandlerTests
    {
        private readonly Mock<IPaymentService> _paymentServiceMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IProfessionalRepository> _professionalRepositoryMock;
        private readonly CreatePaymentIntentCommandHandler _handler;

        public CreatePaymentIntentCommandHandlerTests()
        {
            _paymentServiceMock = new Mock<IPaymentService>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _professionalRepositoryMock = new Mock<IProfessionalRepository>();
            _handler = new CreatePaymentIntentCommandHandler(
                _paymentServiceMock.Object,
                _appointmentRepositoryMock.Object,
                _professionalRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreatePaymentIntent_WhenAppointmentIsValid()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var professionalId = Guid.NewGuid();
            var serviceId = Guid.NewGuid();
            var startTime = DateTime.UtcNow.AddDays(1);
            var price = 100m;

            var appointment = new Appointment(customerId, professionalId, serviceId, startTime, 60, price);
            // Using reflection to set Id since it is protected set
            typeof(Appointment).GetProperty("Id").SetValue(appointment, appointmentId);

            var professional = new Professional(Guid.NewGuid(), "Biz", "Spec");
            professional.UpdateStripeAccountId("acct_123");

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            _professionalRepositoryMock.Setup(x => x.GetByIdAsync(professionalId))
                .ReturnsAsync(professional);

            _paymentServiceMock.Setup(x => x.CreatePaymentIntentAsync(
                It.IsAny<decimal>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<decimal>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, string>>()))
                .ReturnsAsync("pi_123");

            var command = new CreatePaymentIntentCommand(appointmentId, null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal("pi_123", result);
            _paymentServiceMock.Verify(x => x.CreatePaymentIntentAsync(
                price,
                "usd",
                It.Is<string>(s => s.Contains("Appointment for")),
                "acct_123",
                0,
                null,
                It.Is<Dictionary<string, string>>(m => m["AppointmentId"] == appointmentId.ToString())), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAppointmentNotFound()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(appointmentId))
                .ReturnsAsync((Appointment?)null);

            var command = new CreatePaymentIntentCommand(appointmentId, null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAppointmentNotPending()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            var appointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow.AddDays(1), 60, 100);
            appointment.Confirm("pi_old"); // Status becomes Confirmed

            _appointmentRepositoryMock.Setup(x => x.GetByIdAsync(appointmentId))
                .ReturnsAsync(appointment);

            var command = new CreatePaymentIntentCommand(appointmentId, null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
