using System.Text.Json;
using Inventory.Model.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Inventory.Services;

/// <summary>
/// Handles application configuration loading, file creation and database option setup.
/// </summary>
public class ConfigurationService
{
    private const string SHARED_ROOT_FOLDER_NAME = "Fang Software";
    private const string APP_FOLDER_NAME = "Stock Flow";
    private const string APP_SETTINGS_FILE = "appsettings.json";
    private const string SECRETS_FILE = "appsettings.secrets.json";
    private const string TEMPLATE_CONNECTION_STRING = "Your-Database-Connection-String-Here";

    private IConfiguration? configuration;

    /// <summary>
    /// Builds and returns the application configuration from the local app data folder.
    /// </summary>
    public IConfiguration BuildConfiguration()
    {
        EnsureSecretsFileExists();
        EnsureAppSettingsFileExists();

        var fullAppFilePath = Path.Combine(GetApplicationDataPath(), APP_SETTINGS_FILE);
        var fullSecretFilePath = Path.Combine(GetApplicationDataPath(), SECRETS_FILE);

        IConfigurationBuilder configBuilder = new ConfigurationBuilder();

        configBuilder.AddJsonFile(fullAppFilePath, false, true);
        configBuilder.AddJsonFile(fullSecretFilePath, false, true);

        configuration = configBuilder.Build();
        return configuration;
    }

    /// <summary>
    /// Configures the database options using the supplied connection string.
    /// </summary>
    public void ConfigureDatabaseOptions(DbContextOptionsBuilder options, string connectionString)
    {
        options.UseNpgsql(connectionString);

#if DEBUG
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
#endif
    }

    /// <summary>
    /// Configures the database options using the configured connection string.
    /// </summary>
    public void ConfigureDatabaseOptions(DbContextOptionsBuilder options)
    {
        var connectionString = GetConnectionString();

        ConfigureDatabaseOptions(options, connectionString);
    }

    public string GetConnectionString()
    {
        SecretsConfig secrets = GetSecrets();

        if (string.IsNullOrEmpty(secrets.ConnectionString))
        {
            throw new ArgumentNullException(nameof(secrets.ConnectionString),
                "The connection string in the secrets was empty");
        }

        return secrets.ConnectionString;
    }

    /// <summary>
    /// Returns the application data path used for configuration files.
    /// </summary>
    public string GetApplicationDataPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            SHARED_ROOT_FOLDER_NAME,
            APP_FOLDER_NAME);
    }

    /// <summary>
    /// Reads and validates the secrets configuration section.
    /// </summary>
    public SecretsConfig GetSecrets()
    {
        IConfiguration activeConfiguration = configuration ?? BuildConfiguration();

        var config = new SecretsConfig();
        activeConfiguration.GetSection(nameof(SecretsConfig)).Bind(config);

        if (string.IsNullOrEmpty(config.ConnectionString))
        {
            throw new ArgumentNullException(
                nameof(config.ConnectionString),
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

    /// <summary>
    /// Ensures the secrets file exists, creating a template file if needed.
    /// </summary>
    public void EnsureSecretsFileExists()
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

    /// <summary>
    /// Ensures the appsettings file exists, creating an empty one if needed.
    /// </summary>
    public void EnsureAppSettingsFileExists()
    {
        var fullAppFilePath = Path.Combine(GetApplicationDataPath(), APP_SETTINGS_FILE);

        if (File.Exists(fullAppFilePath))
        {
            return;
        }

        var template = new { };

        CreateFile(fullAppFilePath, template);
    }

    /// <summary>
    /// Creates a JSON file with the supplied template content.
    /// </summary>
    public void CreateFile(string path, object template)
    {
        string templateContent = JsonSerializer.Serialize(template, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, templateContent);
    }
}
