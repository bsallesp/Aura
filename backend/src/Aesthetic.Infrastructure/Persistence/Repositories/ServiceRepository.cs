using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aesthetic.Infrastructure.Persistence.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(AestheticDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Service>> GetByProfessionalIdAsync(Guid professionalId)
        {
            return await _dbSet
                .Where(s => s.ProfessionalId == professionalId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Service>> GetActiveServicesAsync()
        {
             return await _dbSet
                .Where(s => s.IsActive)
                .ToListAsync();
        }
    }
}