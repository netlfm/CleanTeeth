using CleanTeeth.Domain.Exceptions;

namespace CleanTeeth.Domain.Common;

public class AuditLog
{
    public Guid Id { get; private set; }
    public string EntityName { get; private set; } = null!;
    public Guid? EntityId { get; private set; }
    public string Action { get; private set; } = null!;
    public string? Changes { get; private set; }
    public Guid? UserId { get; private set; }
    public DateTime Timestamp { get; private set; }
    private AuditLog()
    {
    }
    public AuditLog(string entityName, Guid? entityId, string action, string? changes, Guid? userId)
    {
        if (string.IsNullOrWhiteSpace(entityName))
        {
            throw new BusinessRuleException($"The {nameof(entityName)} is required.");
        }
        if (string.IsNullOrWhiteSpace(action))
        {
            throw new BusinessRuleException($"The {nameof(action)} is required.");
        }

        Id = Guid.CreateVersion7();
        EntityName = entityName.Trim();
        EntityId = entityId;
        Action = action.Trim();
        Changes = changes;
        UserId = userId;
        Timestamp = DateTime.UtcNow;
    }
}
