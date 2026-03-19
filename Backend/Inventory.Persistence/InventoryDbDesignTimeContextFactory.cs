using Inventory.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Inventory.Persistence
{
    /// <summary>
    /// Creates the database context at design time for Entity Framework Core tools.
    /// </summary>
    public class InventoryDbDesignTimeContextFactory : IDesignTimeDbContextFactory<InventoryDatabaseContext>
    {
        /// <inheritdoc />
        public InventoryDatabaseContext CreateDbContext(string[] args)
        {
            var configurationService = new ConfigurationService();

            var configuration = configurationService.BuildConfiguration();

            var optionsBuilder = new DbContextOptionsBuilder<InventoryDatabaseContext>();
            configurationService.ConfigureDatabaseOptions(optionsBuilder);

            return new InventoryDatabaseContext(optionsBuilder.Options);
        }
    }
}
