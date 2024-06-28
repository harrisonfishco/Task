using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Task
{
    public class NonEmptyStringInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            ValidateNonEmptyStrings(eventData.Context!);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ValidateNonEmptyStrings(eventData.Context!);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ValidateNonEmptyStrings(DbContext context)
        {
            try
            {
                foreach (EntityEntry entry in context.ChangeTracker.Entries())
                {
                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        foreach (PropertyEntry property in entry.Properties)
                        {
                            if (property.Metadata.ClrType == typeof(string) && property.CurrentValue is string value && string.IsNullOrEmpty(value))
                            {
                                throw new DbUpdateException($"The property {property.Metadata.Name} cannot be an empty string.");
                            }
                        }
                    }
                }
            } catch(Exception ex) { TaskError.HandleError(ex); }
        }
    }
}
