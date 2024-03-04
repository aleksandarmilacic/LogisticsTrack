using LogisticsTrack.Domain;
using LogisticsTrack.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace LogisticsTrack.Database
{

    public class LogisticsContext : DbContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<GPSRecord> GPSRecords { get; set; }
        public DbSet<TruckPlan> TruckPlans { get; set; }


        public LogisticsContext(DbContextOptions<LogisticsContext> options) : base(options) { }


        // we want to force the use of the soft delete logic
        public override int SaveChanges()
        {
            ApplySoftDeleteLogic();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplySoftDeleteLogic();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplySoftDeleteLogic()
        {
            // prevent soft-deleted entities from being hard-deleted, so we overwrite the state to modified
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is SoftDeletableEntity && e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var softDeletableEntity = (SoftDeletableEntity)entry.Entity;
                softDeletableEntity.SoftDelete();
                entry.State = EntityState.Modified;
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Apply global query filters to all entities inheriting from SoftDeletableEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(SoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = SetGlobalQueryForSoftDeletableEntityMethod.MakeGenericMethod(entityType.ClrType);
                    _ = method.Invoke(this, new object[] { modelBuilder });
                }
            }
        }

        private static readonly MethodInfo SetGlobalQueryForSoftDeletableEntityMethod = typeof(LogisticsContext)
            .GetMethod(nameof(SetGlobalQueryForSoftDeletableEntity), BindingFlags.NonPublic | BindingFlags.Static);

        static void SetGlobalQueryForSoftDeletableEntity<T>(ModelBuilder modelBuilder) where T : SoftDeletableEntity
        {
            modelBuilder.Entity<T>().HasQueryFilter(e => !EF.Property<DateTime?>(e, nameof(SoftDeletableEntity.HasBeenDeleted)).HasValue);
        }
    }
}
