using System.Text.Json;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.ComplexSearchable;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Model.Server;
using Inventory.Persistence;
using Inventory.Persistence.Services;
using Inventory.Server.JsonConverters;
using Inventory.Server.Startup;
using Inventory.Services;
using Inventory.Startup;
using Inventory.Startup.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Inventory.Server;

public class ApiStartup : ModularStartup<IApplicationBuilder>
{
    private readonly IConfiguration configuration;
    private readonly ConfigurationService configurationService;

    public ApiStartup()
    {
        configurationService = new ConfigurationService();
        configuration = configurationService.BuildConfiguration();

        AddModule(new LoggingStartupModule(configurationService.GetApplicationDataPath()));
        AddModule(new SwaggerStartupModule("Inventory"));

        AddModule(new DatabaseContextStartupModule<InventoryDatabaseContext>(configurationService.BuildDatabaseOptions));

        AddModule(new EntityQueryServiceStartupModule<CategoryQueryService, Category, SearchableCategory>());
        AddModule(new EntityQueryServiceStartupModule<LocationItemQueryService, LocationItem, SearchableLocationItem>());
        AddModule(new EntityQueryServiceStartupModule<LocationQueryService, Location, SearchableLocation>());
        AddModule(new EntityQueryServiceStartupModule<OrderItemQueryService, OrderItem, SearchableOrderItem>());
        AddModule(new EntityQueryServiceStartupModule<OrderQueryService, Order, SearchableOrder>());
        AddModule(new EntityQueryServiceStartupModule<ProductQueryService, Product, SearchableProduct>());

        AddModule(new BogusStartupModule());
    }

    /// <inheritdoc />
    protected override void ConfigureApplication(IApplicationBuilder app)
    {
        base.ConfigureApplication(app);

        if (app is not WebApplication webApplication)
            throw new InvalidOperationException(
                $"Expected Supplied App to be of type {nameof(WebApplication)}, but it was a {app.GetType().Name}.");

        webApplication.UseHttpsRedirection();
        webApplication.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(origin => true).AllowCredentials());
        webApplication.UseAuthorization();

        webApplication.MapControllers();
    }

    /// <inheritdoc />
    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);

        services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.Converters.Add(new StringEnumConverter());
            options.SerializerSettings.Converters.Add(new ProductJsonConverter());
        });

        // When a class implementation for a Complex Searchable is added, Add a line below, and update the Type used in the Controller.
        services.AddTransient<IComplexSearchable<SearchableLocationItem>, ComplexSearchableLocationItem>();
        services.AddTransient<IComplexSearchable<SearchableProduct>, ComplexSearchableProduct>();
        services.AddTransient<IComplexSearchable<SearchableCategory>, ComplexSearchableCategory>();

        services.AddCors();
    }
}
