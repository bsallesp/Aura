using Aesthetic.Domain.Common;

namespace Aesthetic.Domain.Entities;

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; private set; }
    public string Action { get; private set; } = null!; // e.g., Appointment.Cancel, Service.UpdatePolicies
    public string ResourceType { get; private set; } = null!; // e.g., Appointment, Service
    public Guid ResourceId { get; private set; }
    public string? Metadata { get; private set; } // JSON string (non-PHI)

    protected AuditLog() { }

    public AuditLog(Guid? userId, string action, string resourceType, Guid resourceId, string? metadata)
    {
        UserId = userId;
        Action = action;
        ResourceType = resourceType;
        ResourceId = resourceId;
        Metadata = metadata;
    }
}
