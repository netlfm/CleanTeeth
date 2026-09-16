using CleanTeeth.Domain.Common;
using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanTeeth.Persistence;

public class CleanTeethDbContext : DbContext
{
    public CleanTeethDbContext(DbContextOptions<CleanTeethDbContext> options) : base(options)
    {

    }
    protected CleanTeethDbContext()
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleanTeethDbContext).Assembly);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "entity");
                var body = Expression.Equal(
                    Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted)),
                    Expression.Constant(false));

                var filter = Expression.Lambda(body, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(filter);
            }
        }
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditableEntity auditable)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        // auditable.CreatedBy = _currentUser.UserId;
                        auditable.CreationTime = now;
                        break;

                    case EntityState.Modified:
                        //auditable.ModifiedBy = _currentUser.UserId;
                        auditable.LastModifiedDate = now;
                        break;
                }
            }
            if (entry.Entity is ISoftDeletable deletable && entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                deletable.IsDeleted = true;
                //deletable.DeletedBy = _currentUser.UserId;
                deletable.DeletedAt = now;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
    public DbSet<DentalOffice> DentalOffices { get; set; }
    public DbSet<Dentist> Dentists { get; set; }

}
