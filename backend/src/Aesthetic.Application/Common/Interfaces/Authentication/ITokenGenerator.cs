using Aesthetic.Domain.Entities;

namespace Aesthetic.Application.Common.Interfaces.Authentication
{
    public interface ITokenGenerator
    {
        string GenerateToken(User user);
    }
}