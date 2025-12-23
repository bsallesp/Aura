using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aesthetic.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AestheticDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}