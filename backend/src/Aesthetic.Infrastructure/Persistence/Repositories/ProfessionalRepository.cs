using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aesthetic.Infrastructure.Persistence.Repositories
{
    public class ProfessionalRepository : Repository<Professional>, IProfessionalRepository
    {
        public ProfessionalRepository(AestheticDbContext context) : base(context)
        {
        }

        public async Task<Professional?> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Availabilities)
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }

        public async Task<Professional?> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(p => p.User)
                .Include(p => p.Availabilities)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Professional?> GetByStripeAccountIdAsync(string stripeAccountId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.StripeAccountId == stripeAccountId);
        }

        public async Task<IEnumerable<Professional>> GetBySpecialtyAsync(string specialty)
        {
            return await _dbSet
                .Include(p => p.User)
                .Where(p => p.Specialty != null && p.Specialty.Contains(specialty))
                .ToListAsync();
        }
    }
}