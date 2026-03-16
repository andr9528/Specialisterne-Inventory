using System.Text.Json;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.ComplexSearchable;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Model.Server;
using Inventory.Persistence;
using Inventory.Persistence.Services;
using Inventory.Server.Startup;
using Inventory.Startup;
using Inventory.Startup.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Inventory.Server;

public class ApiStartup : ModularStartup<IApplicationBuilder>
{
    private const string SHARED_ROOT_FOLDER_NAME = "Fang Software";
    private const string APP_FOLDER_NAME = "Stock Flow";
    private const string APP_SETTINGS_FILE = "appsettings.json";
    private const string SECRETS_FILE = "appsettings.secrets.json";
    private const string TEMPLATE_CONNECTION_STRING = "Your-Database-Connection-String-Here";

    private readonly IConfiguration configuration;

    public ApiStartup()
    {
        configuration = BuildConfiguration();

        AddModule(new LoggingStartupModule(GetApplicationDataPath()));
        AddModule(new SwaggerStartupModule("Inventory"));

        AddModule(new DatabaseContextStartupModule<InventoryDatabaseContext>(GetDatabaseOptions));

        AddModule(new EntityQueryServiceStartupModule<CategoryQueryService, Category, SearchableCategory>());
        AddModule(new EntityQueryServiceStartupModule<LocationItemQueryService, LocationItem, SearchableLocationItem>());
        AddModule(new EntityQueryServiceStartupModule<LocationQueryService, Location, SearchableLocation>());
        AddModule(new EntityQueryServiceStartupModule<OrderItemQueryService, OrderItem, SearchableOrderItem>());
        AddModule(new EntityQueryServiceStartupModule<OrderQueryService, Order, SearchableOrder>());
        AddModule(new EntityQueryServiceStartupModule<ProductQueryService, Product, SearchableProduct>());
    }

    private void GetDatabaseOptions(DbContextOptionsBuilder options)
    {
        SecretsConfig secrets = GetSecrets();

        if (string.IsNullOrEmpty(secrets.ConnectionString))
        {
            throw new ArgumentNullException(nameof(secrets.ConnectionString), "The connection string in the secrets was empty");
        }

        options.UseNpgsql(secrets.ConnectionString);

#if DEBUG
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
#endif
    }

    private SecretsConfig GetSecrets()
    {
        var config = new SecretsConfig();
        configuration.GetSection(nameof(SecretsConfig)).Bind(config);

        if (string.IsNullOrEmpty(config.ConnectionString))
        {
            throw new ArgumentNullException(nameof(config.ConnectionString),
                "The connection string in the secrets was empty");
        }

        if (config.ConnectionString.Equals(TEMPLATE_CONNECTION_STRING, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new ArgumentException(
                $"The connection string in the secrets was not set. It is still the template value '{TEMPLATE_CONNECTION_STRING}'",
                nameof(config.ConnectionString));
        }

        return config;
    }

    private IConfiguration BuildConfiguration()
    {
        EnsureSecretsFileExists();
        EnsureAppSettingsFileExist();

        var fullAppFilePath = Path.Combine(GetApplicationDataPath(), APP_SETTINGS_FILE);
        var fullSecretFilePath = Path.Combine(GetApplicationDataPath(), SECRETS_FILE);

        IConfigurationBuilder configBuilder = new ConfigurationBuilder();

        configBuilder.AddJsonFile(fullAppFilePath, false, true);
        configBuilder.AddJsonFile(fullSecretFilePath, false, true);

        return configBuilder.Build();
    }

    private void EnsureSecretsFileExists()
    {
        var fullSecretFilePath = Path.Combine(GetApplicationDataPath(), SECRETS_FILE);

        if (File.Exists(fullSecretFilePath))
        {
            return;
        }

        var template = new
        {
            SecretsConfig = new SecretsConfig
            {
                ConnectionString = TEMPLATE_CONNECTION_STRING,
            },
        };

        CreateFile(fullSecretFilePath, template);

        throw new FileNotFoundException(
            $"Secrets file '{fullSecretFilePath}' was not found. An empty template secrets file as been generated. Please fill it out before running the application again.");
    }

    private void EnsureAppSettingsFileExist()
    {
        var fullAppFilePath = Path.Combine(GetApplicationDataPath(), APP_SETTINGS_FILE);

        if (File.Exists(fullAppFilePath))
        {
            return;
        }

        var template = new { };

        CreateFile(fullAppFilePath, template);
    }

    private void CreateFile(string path, object template)
    {
        string templateContent = JsonSerializer.Serialize(template, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, templateContent);
    }

    private string GetApplicationDataPath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            SHARED_ROOT_FOLDER_NAME, APP_FOLDER_NAME);
    }

    /// <inheritdoc />
    protected override void ConfigureApplication(IApplicationBuilder app)
    {
        base.ConfigureApplication(app);

        if (app is not WebApplication webApplication)
            throw new InvalidOperationException(
                $"Expected Supplied App to be of type {nameof(WebApplication)}, but it was a {app.GetType().Name}.");

        webApplication.UseHttpsRedirection();
        webApplication.UseAuthorization();
        webApplication.MapControllers();
    }

    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddControllers();

        // When a class implementation for a Complex Searchable is added, Add a line below, and update the Type used in the Controller.
        services.AddTransient<IComplexSearchable<SearchableLocationItem>, ComplexSearchableLocationItem>();
    }
}
