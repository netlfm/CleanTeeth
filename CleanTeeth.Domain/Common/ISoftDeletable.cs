namespace CleanTeeth.Domain.Common;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    Guid? DeletedBy { get; }
    DateTime? DeletedAt { get; }
}
