using Aesthetic.Domain.Common;
using Aesthetic.Domain.Entities;

namespace Aesthetic.Domain.Events
{
    public record UserRegisteredEvent(User User) : IDomainEvent;
}
