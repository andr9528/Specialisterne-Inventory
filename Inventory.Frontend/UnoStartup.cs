using System.Text.Json;
using Inventory.Frontend;
using Inventory.Frontend.Models;
using Inventory.Frontend.Services.Endpoints;
using Inventory.Startup;
using Inventory.Startup.Modules;
using Microsoft.UI.Dispatching;
using Uno.Extensions;
using Windows.Phone.System.UserProfile.GameServices.Core;
using Path = System.IO.Path;

namespace Inventory.Frontend;

public class UnoStartup : ModularStartup<IApplicationBuilder>
{
    private const string SHARED_ROOT_FOLDER_NAME = "Fang Software";
    private const string APP_FOLDER_NAME = "Stock Flow";

    public UnoStartup()
    {
        //AddModule(new LoggingStartupModule(GetApplicationDataPath()));
    }

    private string GetApplicationDataPath()
    {
        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            SHARED_ROOT_FOLDER_NAME, APP_FOLDER_NAME);
    }

    protected IHost? Host { get; private set; }

    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        //var uiDispatcherQueue = DispatcherQueue.GetForCurrentThread();
        //services.AddSingleton<IUiDispatcher>(new UiDispatcher(uiDispatcherQueue));
    }

    /// <inheritdoc />
    protected override void ConfigureApplication(IApplicationBuilder app)
    {
        app.Configure(host => host
#if DEBUG
            // Switch to Development environment when running in DEBUG
            //.UseEnvironment(Environments.Development)
#endif
            .UseConfiguration(configure: ConfigureConfigurationSource)
            .UseLocalization(ConfigureLocalization)
            .UseSerialization(ConfigureSerialization));
            //.UseAuthentication(ConfigureAuthentication)
            //.UseHttp(ConfigureHttp));

        base.ConfigureApplication(app);

        Host = app.Build();
    }

    private void ConfigureHttp(HostBuilderContext host, IServiceCollection services)
    {
#if DEBUG
        // DelegatingHandler will be automatically injected
        //services.AddTransient<DelegatingHandler, DebugHttpHandler>();
#endif
        //services.AddSingleton<IWeatherCache, WeatherCache>();
        services.AddRefitClient<IApiClient>(host);
    }

    private void ConfigureAuthentication(IAuthenticationBuilder auth)
    {
        //auth.AddCustom(custom => custom.Login(AttemptLogin).Refresh(AttemptRefreshLogin), name: "CustomAuth");
    }

    private ValueTask<IDictionary<string, string>?> AttemptRefreshLogin(IServiceProvider sp, IDictionary<string, string> tokenDictionary, CancellationToken cancellationToken)
    {
        // TODO: Write code to refresh tokens using the currently stored tokens
        if ((tokenDictionary?.TryGetValue(TokenCacheExtensions.RefreshTokenKey, out var refreshToken) ?? false) && !refreshToken.IsNullOrEmpty() && (tokenDictionary?.TryGetValue("Expiry", out var expiry) ?? false) && DateTime.TryParse(expiry, out var tokenExpiry) && tokenExpiry > DateTime.Now)
        {
            // Return IDictionary containing any tokens used by service calls or in the app
            tokenDictionary ??= new Dictionary<string, string>();
            tokenDictionary[TokenCacheExtensions.AccessTokenKey] = "NewSampleToken";
            tokenDictionary["Expiry"] = DateTime.Now.AddMinutes(5).ToString("g");
            return ValueTask.FromResult<IDictionary<string, string>?>(tokenDictionary);
        }

        // Return null/default to fail the Refresh method
        return ValueTask.FromResult<IDictionary<string, string>?>(default);
    }

    private ValueTask<IDictionary<string, string>?> AttemptLogin(IServiceProvider sp, IDispatcher? dispatcher, IDictionary<string, string> credentials, CancellationToken cancellationToken)
    {
        //// TODO: Write code to process credentials that are passed into the LoginAsync method
        //if (credentials?.TryGetValue(nameof(LoginViewModel.Username), out var username) ?? false && !username.IsNullOrEmpty())
        //{
        //    // Return IDictionary containing any tokens used by service calls or in the app
        //    credentials ??= new Dictionary<string, string>();
        //    credentials[TokenCacheExtensions.AccessTokenKey] = "SampleToken";
        //    credentials[TokenCacheExtensions.RefreshTokenKey] = "RefreshToken";
        //    credentials["Expiry"] = DateTime.Now.AddMinutes(5).ToString("g");
        //    return ValueTask.FromResult<IDictionary<string, string>?>(credentials);
        //}

        // Return null/default to fail the LoginAsync method
        return ValueTask.FromResult<IDictionary<string, string>?>(default);
    }

    private void ConfigureSerialization(HostBuilderContext host, IServiceCollection services)
    {
        //services.AddContentSerializer(host)
        //    .AddJsonTypeInfo(WeatherForecastContext.Default.IImmutableListWeatherForecast);

        services.AddSingleton(new JsonSerializerOptions { IncludeFields = true, });
    }

    private IHostBuilder ConfigureConfigurationSource(IConfigBuilder configBuilder)
    {
        return configBuilder.EmbeddedSource<App>().Section<AppConfig>();
    }

    private void ConfigureLocalization(HostBuilderContext context, IServiceCollection services)
    {
        // Enables localization (see appsettings.json for supported languages)
    }
}
