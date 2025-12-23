using Aesthetic.Domain.Entities;

namespace Aesthetic.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}