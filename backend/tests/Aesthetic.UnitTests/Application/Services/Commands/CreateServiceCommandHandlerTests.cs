using Aesthetic.Application.Services.Commands.CreateService;
using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Moq;
using Xunit;

namespace Aesthetic.UnitTests.Application.Services.Commands
{
    public class CreateServiceCommandHandlerTests
    {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateServiceCommandHandler _handler;

        public CreateServiceCommandHandlerTests()
        {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new CreateServiceCommandHandler(_serviceRepositoryMock.Object, _unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateService_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateServiceCommand(
                ProfessionalId: Guid.NewGuid(),
                Name: "Botox",
                Price: 150.00m,
                DurationMinutes: 30,
                Description: "Botox treatment"
            );

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Name, result.Name);
            Assert.Equal(command.Price, result.Price);
            Assert.Equal(command.DurationMinutes, result.DurationMinutes);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.ProfessionalId, result.ProfessionalId);
            Assert.True(result.IsActive);

            _serviceRepositoryMock.Verify(x => x.AddAsync(It.Is<Service>(s => 
                s.Name == command.Name && 
                s.ProfessionalId == command.ProfessionalId
            )), Times.Once);

            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
