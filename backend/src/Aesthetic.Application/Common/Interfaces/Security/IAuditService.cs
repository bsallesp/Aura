namespace Aesthetic.Application.Common.Interfaces.Security;

public interface IAuditService
{
    Task LogAsync(Guid? userId, string action, string resourceType, Guid resourceId, string? metadata = null);
}
