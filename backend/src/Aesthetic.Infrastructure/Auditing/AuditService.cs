using Aesthetic.Application.Common.Interfaces.Security;
using Aesthetic.Domain.Entities;
using Aesthetic.Domain.Interfaces;

namespace Aesthetic.Infrastructure.Auditing;

public class AuditService : IAuditService
{
    private readonly IAuditLogRepository _repo;
    private readonly IUnitOfWork _uow;

    public AuditService(IAuditLogRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task LogAsync(Guid? userId, string action, string resourceType, Guid resourceId, string? metadata = null)
    {
        var log = new AuditLog(userId, action, resourceType, resourceId, metadata);
        await _repo.AddAsync(log);
        await _uow.SaveChangesAsync();
    }
}
