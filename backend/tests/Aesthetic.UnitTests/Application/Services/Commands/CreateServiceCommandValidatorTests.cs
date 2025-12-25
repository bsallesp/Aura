using Aesthetic.Application.Services.Commands.CreateService;

namespace Aesthetic.UnitTests.Application.Services.Commands
{
    public class CreateServiceCommandValidatorTests
    {
        private readonly CreateServiceCommandValidator _validator;

        public CreateServiceCommandValidatorTests()
        {
            _validator = new CreateServiceCommandValidator();
        }

        [Fact]
        public void Validate_ShouldReturnTrue_WhenCommandIsValid()
        {
            var command = new CreateServiceCommand(
                Guid.NewGuid(),
                "Valid Service",
                100m,
                60,
                "Description"
            );

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Validate_ShouldFail_WhenNameIsInvalid(string? name)
        {
            var command = new CreateServiceCommand(
                Guid.NewGuid(),
                name!,
                100m,
                60,
                null
            );

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        }

        [Fact]
        public void Validate_ShouldFail_WhenPriceIsZeroOrNegative()
        {
            var command = new CreateServiceCommand(
                Guid.NewGuid(),
                "Service",
                0m,
                60,
                null
            );

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Price");
        }

        [Fact]
        public void Validate_ShouldFail_WhenDurationIsInvalid()
        {
            var command = new CreateServiceCommand(
                Guid.NewGuid(),
                "Service",
                100m,
                0, // Invalid duration
                null
            );

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "DurationMinutes");
        }
    }
}
