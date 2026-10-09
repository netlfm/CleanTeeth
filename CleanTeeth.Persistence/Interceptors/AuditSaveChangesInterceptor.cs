using CleanTeeth.Application.Contracts.Security;
using CleanTeeth.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace CleanTeeth.Persistence.Interceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IUserService _userService;
    public AuditSaveChangesInterceptor(IUserService userService)
    {
        _userService = userService;
    }
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken)
    {
        var context = eventData.Context; 
        if (context is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .Where(e => e.Entity is not AuditLog)
            .ToList();
        foreach (var entry in entries)
        {
            var auditLog = CreateAuditLog(entry);
            context.Add(auditLog);
        }
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    private AuditLog CreateAuditLog(EntityEntry entry)
    {
        var entityName = entry.Metadata.ClrType.Name;
        var entityId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue as Guid?;
        var action = entry.State
            switch
        {
            EntityState.Added => "Created",
            EntityState.Modified => "Updated",
            EntityState.Deleted => "Deleted",
            _ => "Unknown"
        };
        string changes = string.Empty;
        if (entry.State == EntityState.Modified)
        {
            var modifiedProperties = entry.Properties
                .Where(p => p.IsModified)
                .Where(p => !Equals(p.OriginalValue, p.CurrentValue))
                .Select(p => new
                {
                    Property = p.Metadata.Name,
                    OldValue = p.OriginalValue?.ToString(),
                    NewValue = p.CurrentValue?.ToString()
                }).ToList();
            if (modifiedProperties.Count > 0)
            {
                changes = JsonSerializer.Serialize(modifiedProperties);
            }
        }
        return new AuditLog(entityName, entityId, action, changes, _userService.UserId);
    }
}
