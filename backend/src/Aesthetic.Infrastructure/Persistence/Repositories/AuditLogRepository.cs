using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;

namespace Aesthetic.Infrastructure.Persistence.Repositories;

public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
{
    public AuditLogRepository(AestheticDbContext context) : base(context) { }
}
