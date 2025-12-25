using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aesthetic.Infrastructure.Persistence.Repositories;

public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(AestheticDbContext context) : base(context) { }

    public async Task<int> DeleteOlderThanAsync(DateTime cutoffDate)
    {
        return await _dbSet
            .Where(x => x.CreatedAt < cutoffDate)
            .ExecuteDeleteAsync();
    }
}
