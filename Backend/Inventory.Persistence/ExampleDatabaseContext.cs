using Microsoft.EntityFrameworkCore;
using Inventory.Model.Entity;
using Inventory.Persistence.Configuration;
using Inventory.Persistence.Core;
using Inventory.Persistence.Core.Abstraction;

namespace Inventory.Persistence
{
    public class ExampleDatabaseContext : BaseDatabaseContext<ExampleDatabaseContext>
    {
        /// <inheritdoc />
        public ExampleDatabaseContext(DbContextOptions<ExampleDatabaseContext> options) : base(options)
        {
        }

        // Use of 'Virtual' allows Entity Framework Core to create proxy classes for lazy loading, which can improve performance by only loading related data when it's actually needed.
        public virtual DbSet<Example> Examples { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var databaseType =
                DatabaseType.SQL_LITE; // This can be made dynamic based on configuration or environment variables.

            modelBuilder.ApplyConfiguration(new ExampleConfiguration(databaseType));
        }
    }
}
