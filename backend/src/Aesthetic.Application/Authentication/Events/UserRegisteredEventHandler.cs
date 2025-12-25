using Aesthetic.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Aesthetic.Application.Authentication.Events
{
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
    {
        private readonly ILogger<UserRegisteredEventHandler> _logger;

        public UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: User registered with ID {UserId} and Email {Email}. Sending welcome email...", 
                notification.User.Id, notification.User.Email);

            // Logic to send email would go here.
            
            return Task.CompletedTask;
        }
    }
}
