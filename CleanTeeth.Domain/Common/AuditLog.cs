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
            throw new ArgumentException("Entity name is required.", nameof(entityName));
        }
        if (string.IsNullOrWhiteSpace(action))
        {
            throw new ArgumentException("Action is required.", nameof(action));
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
