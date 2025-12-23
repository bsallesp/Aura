using Xunit;
using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Enums;
using System;

namespace Aesthetic.UnitTests.Domain
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_WithValidData_ShouldSucceed()
        {
            var user = new User("John", "Doe", "john@example.com", "hash", UserRole.Customer);

            Assert.NotNull(user);
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal(UserRole.Customer, user.Role);
            Assert.NotEqual(Guid.Empty, user.Id);
        }

        [Theory]
        [InlineData("", "Doe", "email")]
        [InlineData("John", "", "email")]
        [InlineData("John", "Doe", "")]
        public void CreateUser_WithInvalidData_ShouldThrowException(string firstName, string lastName, string email)
        {
            Assert.Throws<ArgumentException>(() => new User(firstName, lastName, email, "hash", UserRole.Customer));
        }
    }

    public class AppointmentTests
    {
        [Fact]
        public void CreateAppointment_InPast_ShouldThrowException()
        {
            var pastDate = DateTime.UtcNow.AddHours(-1);
            Assert.Throws<ArgumentException>(() => new Appointment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), pastDate, 60, 50));
        }

        [Fact]
        public void ConfirmAppointment_ShouldSetStatusAndPaymentId()
        {
            var futureDate = DateTime.UtcNow.AddHours(1);
            var appointment = new Appointment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), futureDate, 60, 50);

            appointment.Confirm("pi_123");

            Assert.Equal(AppointmentStatus.Confirmed, appointment.Status);
            Assert.Equal("pi_123", appointment.StripePaymentIntentId);
        }
    }
}
