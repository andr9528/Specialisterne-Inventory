using Inventory.Abstraction.Interfaces.Startup;
using Inventory.Persistence;

namespace Inventory.Server.Startup;

public class BogusStartupModule : IServiceStartupModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        ServiceProvider? provider = services.BuildServiceProvider();
        using (var context = provider.GetService<InventoryDatabaseContext>())
        {
            //If no Categories, assume no data. Only table that does not depend on Categories is Locations
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(Services.BogusService.GetCategories(10));
                context.Locations.AddRange(Services.BogusService.GetLocations(10));
                context.SaveChanges();

                context.Products.AddRange(Services.BogusService.GetProducts(100, context.Categories));
                context.Orders.AddRange(Services.BogusService.GetOrders(10, context.Locations));
                context.SaveChanges();

                context.LocationItems.AddRange(Services.BogusService.GetLocationItemsForEach(context.Locations, context.Products));
                context.OrderItems.AddRange(Services.BogusService.GetOrderItems(10, context.Orders, context.Products.ToList()));
                context.SaveChanges();
            }
        }
    }
}

