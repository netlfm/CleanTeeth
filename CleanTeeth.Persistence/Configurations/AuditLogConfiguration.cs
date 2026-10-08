using CleanTeeth.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanTeeth.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.EntityName)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Action)
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(x => x.Changes)
            .HasColumnType("nvarchar(max)");
        builder.HasIndex(x => x.EntityName);
        builder.HasIndex(x => x.EntityId);
        builder.HasIndex(x => x.Timestamp);
    }
}
