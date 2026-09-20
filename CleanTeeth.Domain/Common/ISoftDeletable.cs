namespace CleanTeeth.Domain.Common;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    string? DeletedBy { get; }
    DateTime? DeletedAt { get; }
}
