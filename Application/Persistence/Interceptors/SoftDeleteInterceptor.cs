using EfCoreDemo.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EfCoreDemo.Persistence.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return result;
        }

        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry is not { State: EntityState.Deleted, Entity: ISoftDeleteEntity deleted })
            {
                continue;
            }

            entry.State = EntityState.Modified;
            deleted.IsDeleted = true;
            deleted.DeletedAt = DateTimeOffset.UtcNow;
        }

        return result;
    }
}