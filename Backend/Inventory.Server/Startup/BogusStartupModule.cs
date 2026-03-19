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
                var c = Services.BogusService.GetCategories(10);
                var l = Services.BogusService.GetLocations(10);
                var p = Services.BogusService.GetProducts(100, c);
                var o = Services.BogusService.GetOrders(10, l);

                context.AddRange(o);
                context.AddRange(c);
                context.AddRange(l);
                context.AddRange(p);
                context.AddRange(Services.BogusService.GetLocationItemsForEach(l, p));
                context.AddRange(Services.BogusService.GetOrderItems(10, o, p.ToList()));
                context.SaveChanges();
            }
        }
    }
}

