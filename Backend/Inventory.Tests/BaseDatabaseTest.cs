using Inventory.Persistence;
using Inventory.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Inventory.Tests;

public abstract class BaseDatabaseTest
{
    private const string TEST_DATABASE = "inventory_test";
    protected InventoryDatabaseContext context;
    private ConfigurationService configurationService;
    protected BaseDatabaseTest()
    {
        configurationService = new ConfigurationService();
        _ = configurationService.BuildConfiguration();

        var optionsBuilder = new DbContextOptionsBuilder<InventoryDatabaseContext>();
        var connectionString = ReplaceDatabase(configurationService.GetConnectionString(), TEST_DATABASE);
        configurationService.ConfigureDatabaseOptions(optionsBuilder, connectionString);
        context = new InventoryDatabaseContext(optionsBuilder.Options);
    }

    private string ReplaceDatabase(string connectionString, string testDatabaseName)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = testDatabaseName
        };

        return builder.ConnectionString;
    }
}
