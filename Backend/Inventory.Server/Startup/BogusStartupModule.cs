using Bogus;
using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Startup;
using Inventory.Model.Entity;
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
                context.Categories.AddRange(FakeData.GetCategories());
                context.Locations.AddRange(FakeData.GetLocations());
                context.SaveChanges();

                context.Products.AddRange(FakeData.GetProducts(context.Categories.ToList()));
                context.Orders.AddRange(FakeData.GetOrders(context.Locations.ToList()));
                context.SaveChanges();

                context.LocationItems.AddRange(FakeData.GetLocationItems(context.Locations.ToList(), context.Products.ToList()));
                context.OrderItems.AddRange(FakeData.GetOrderItems(context.Orders.ToList(), context.Products.ToList()));
                context.SaveChanges();
            }
        }
    }
}

public static class FakeData
{
    public static List<Category> Categories = new List<Category>();
    public static List<Product> Products = new List<Product>();
    public static List<Location> Locations = new List<Location>();
    public static List<LocationItem> LocationItems = new List<LocationItem>();
    public static List<OrderItem> OrderItems = new List<OrderItem>();
    public static List<Order> Orders = new List<Order>();

    public static IEnumerable<Category> GetCategories()
    {
        var categoryFaker = new Faker<Category>()
            .RuleFor(c => c.Name, f => f.Commerce.Department());
        return categoryFaker.Generate(5);
    }


    internal static IEnumerable<Location> GetLocations()
    {
        var locationFaker = new Faker<Location>()
            .RuleFor(l => l.Name, f => f.Company.CompanyName());
        return locationFaker.Generate(5);
    }

    internal static IEnumerable<Product> GetProducts(IEnumerable<Category> categories)
    {
        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Category, f => f.PickRandom(categories))
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
            ;
        return productFaker.Generate(25);
    }

    internal static IEnumerable<LocationItem> GetLocationItems(List<Location> locations, List<Product> products)
    {
        var locationItemFaker = new Faker<LocationItem>()
            .RuleFor(li => li.Product, f => f.PickRandom(products))
            .RuleFor(li => li.Location, f => f.PickRandom(locations))
            .RuleFor(li => li.Quantity, f => f.Random.Number(0, 1000))
            .RuleFor(li => li.TargetQuantity, f => f.Random.Number(100, 500))
            .RuleFor(li => li.ReservedQuantity, f => f.Random.Number(0, 50))
            .RuleFor(li => li.Status, f => f.PickRandom<InventoryStatus>());
        return locationItemFaker.Generate(100);
    }

    internal static IEnumerable<Order> GetOrders(List<Location> locations)
    {
        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.Status, f => f.PickRandom<OrderStatus>())
            .RuleFor(o => o.ReferenceId, f => Guid.NewGuid())
            //Perhaps remove the OrNull?
            .RuleFor(o => o.Location, f => f.PickRandom(locations).OrNull(f))
            ;

        return orderFaker.Generate(5);
    }


    internal static IEnumerable<OrderItem> GetOrderItems(List<Order> orders, List<Product> products)
    {
        var orderItemFaker = new Faker<OrderItem>()
            .RuleFor(oi => oi.Product, f => f.PickRandom(products))
            .RuleFor(oi => oi.Order, f => f.PickRandom(orders))
            .RuleFor(oi => oi.Quantity, f => f.Random.Number(1, 10));

        return orderItemFaker.Generate(30);
    }
}
