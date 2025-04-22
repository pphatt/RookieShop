﻿using HeadphoneStore.Domain.Abstracts.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace HeadphoneStore.API.Interceptors;

public sealed class UpdateAuditableEntitiesInterceptor
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(
                eventData,
                result,
                cancellationToken);
        }

        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            dbContext
                .ChangeTracker
                .Entries<IAuditableEntity>();
        TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(a => a.CreatedDateTime).CurrentValue
                    = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, vietnamTimeZone);
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(a => a.UpdatedDateTime).CurrentValue
                    = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, vietnamTimeZone);
            }
        }

        return base.SavingChangesAsync(
            eventData,
            result,
            cancellationToken);
    }
}