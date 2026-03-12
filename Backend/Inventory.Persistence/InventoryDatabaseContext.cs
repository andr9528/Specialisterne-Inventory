using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Model.Entity;
using Inventory.Persistence.Configuration;
using Inventory.Persistence.Core;
using Inventory.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence
{
    public class InventoryDatabaseContext : BaseDatabaseContext<InventoryDatabaseContext>
    {
        /// <inheritdoc />
        public InventoryDatabaseContext(DbContextOptions<InventoryDatabaseContext> options) : base(options)
        {
        }

        // Use of 'Virtual' allows Entity Framework Core to create proxy classes for lazy loading, which can improve performance by only loading related data when it's actually needed.
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationItem> LocationItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var databaseType =
                DatabaseType.POSTGRESQL; // This can be made dynamic based on configuration or environment variables.

            modelBuilder.ApplyConfiguration(new ExampleConfiguration(databaseType));
        }
    }
}
