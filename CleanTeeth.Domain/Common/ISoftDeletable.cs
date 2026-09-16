namespace CleanTeeth.Domain.Common;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    string? DeletedBy { get; set; }
    DateTime? DeletedAt { get; set; }
}
