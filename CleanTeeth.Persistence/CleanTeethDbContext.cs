using CleanTeeth.Application.Contracts.Security;
using CleanTeeth.Domain.Common;
using CleanTeeth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanTeeth.Persistence;

public class CleanTeethDbContext : DbContext
{
    private readonly IUserService _userService;
    public CleanTeethDbContext(DbContextOptions<CleanTeethDbContext> options, IUserService userService) : base(options)
    {
        _userService = userService;
    }
    //protected CleanTeethDbContext()
    //{

    //}
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
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var currentUserId = _userService.UserId;
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditableEntity auditable)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditable.CreatedBy = currentUserId;
                        auditable.CreationTime = now;
                        break;

                    case EntityState.Modified:
                        auditable.LastModifiedBy = currentUserId;
                        auditable.LastModifiedDate = now;
                        break;
                }
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
    public DbSet<DentalOffice> DentalOffices { get; set; }
    public DbSet<Dentist> Dentists { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
}
