namespace CleanTeeth.Domain.Common;

public abstract class AuditableEntity
{
    public Guid? CreatedBy { get; set; }
    public DateTime? CreationTime { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModifiedDate { get; set; }
}
