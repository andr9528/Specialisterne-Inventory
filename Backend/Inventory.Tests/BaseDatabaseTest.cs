using Inventory.Persistence;
using Inventory.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;

namespace Inventory.Tests;

public abstract class BaseDatabaseTest
{
    private static readonly string TestDatabaseName = $"inventory_test_{Guid.NewGuid():N}";
    private IDbContextFactory<InventoryDatabaseContext> contextFactory;
    private readonly ConfigurationService configurationService;
    private bool disposed;

    protected BaseDatabaseTest()
    {
        configurationService = new ConfigurationService();
        _ = configurationService.BuildConfiguration();

        var optionsBuilder = new DbContextOptionsBuilder<InventoryDatabaseContext>();

        var connectionString = ReplaceDatabase(configurationService.GetConnectionString(), TestDatabaseName);

        configurationService.ConfigureDatabaseOptions(optionsBuilder, connectionString);

        contextFactory = new PooledDbContextFactory<InventoryDatabaseContext>(optionsBuilder.Options);

        using var context = CreateContext();
        context.Database.Migrate();
    }

    protected InventoryDatabaseContext CreateContext()
    {
        return contextFactory.CreateDbContext();
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        using var context = CreateContext();
        context.Database.EnsureDeleted();

        disposed = true;
        GC.SuppressFinalize(this);
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
