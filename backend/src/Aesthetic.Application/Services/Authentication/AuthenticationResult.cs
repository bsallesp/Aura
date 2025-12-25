using Aesthetic.Domain.Entities;

namespace Aesthetic.Application.Services.Authentication
{
    public record AuthenticationResult(
        User User,
        string Token
    );
}
