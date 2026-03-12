using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Inventory.Persistence
{
    // Todo: Figure out how to get the connection string from the `appsettings.secrets.json` file at design time.
    public class InventoryDbDesignTimeContextFactory : IDesignTimeDbContextFactory<InventoryDatabaseContext>
    {
        /// <inheritdoc />
        public InventoryDatabaseContext CreateDbContext(string[] args)
        {
            // In a real implementation the DbContext needs to be configured with the
            // same provider used by the application (for example SQLite).

            // Example of how a SQLite path could be constructed:
            //
            // var basePath = Directory.GetCurrentDirectory();
            // var dbPath = Path.Combine(basePath, InventoryDatabaseContext.CONNECTION_STRING);

            var optionsBuilder = new DbContextOptionsBuilder<InventoryDatabaseContext>();

            // Example of configuring SQLite for migrations:
            //
            // optionsBuilder.UseSqlite($"Data Source={dbPath}");

            // In this template example we intentionally do NOT configure a provider.
            // This allows the code to compile while still demonstrating where the
            // configuration would normally occur.

            return new InventoryDatabaseContext(optionsBuilder.Options);
        }
    }
}
