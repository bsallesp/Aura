using Aesthetic.API.Controllers;
using Aesthetic.Application.Connect.Commands.StartOnboarding;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Claims;

namespace Aesthetic.UnitTests.Controllers
{
    public class ConnectControllerTests
    {
        private readonly Mock<ISender> _senderMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly ConnectController _controller;

        public ConnectControllerTests()
        {
            _senderMock = new Mock<ISender>();
            _configurationMock = new Mock<IConfiguration>();

            _controller = new ConnectController(
                _senderMock.Object,
                _configurationMock.Object
            );
        }

        [Fact]
        public async Task StartOnboarding_ShouldReturnOk_WhenCommandSucceeds()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var accountLinkUrl = "https://connect.stripe.com/setup/s/acct_123";

            // Mock User Claims
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            _senderMock.Setup(x => x.Send(It.IsAny<StartOnboardingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(accountLinkUrl);

            // Act
            var result = await _controller.StartOnboarding();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}
