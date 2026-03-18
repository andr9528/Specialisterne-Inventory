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
            .UseSeed(12121)
            .RuleFor(c => c.Name, f => f.Commerce.Department());
        return categoryFaker.Generate(10);
    }


    internal static IEnumerable<Location> GetLocations()
    {
        var locationFaker = new Faker<Location>()
            .UseSeed(23232)
            .RuleFor(l => l.Name, f => f.Company.CompanyName());
        return locationFaker.Generate(10);
    }

    internal static IEnumerable<Product> GetProducts(IEnumerable<Category> categories)
    {
        var productFaker = new Faker<Product>()
            .UseSeed(34343)
            .RuleFor(p => p.Category, f => f.PickRandom(categories))
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
            ;
        return productFaker.Generate(100);
    }

    internal static IEnumerable<LocationItem> GetLocationItems(List<Location> locations, List<Product> products)
    {
        var locationItemFaker = new Faker<LocationItem>()
            .UseSeed(45454)
            .RuleFor(li => li.Quantity, f => f.Random.Number(0, 1000))
            .RuleFor(li => li.TargetQuantity, f => f.Random.Number(100, 500))
            .RuleFor(li => li.ReservedQuantity, f => f.Random.Number(0, 50));
        foreach (var location in locations)
        {
            foreach(var product in products)
            {
                var locationItem = locationItemFaker.Generate(1).First();
                locationItem.Location = location;
                locationItem.Product = product;
                yield return locationItem;
            }
        }
    }

    internal static IEnumerable<Order> GetOrders(List<Location> locations)
    {
        var orderFaker = new Faker<Order>()
            .UseSeed(56565)
            .RuleFor(o => o.Status, f => f.PickRandom<OrderStatus>())
            .RuleFor(o => o.ReferenceId, f => Guid.NewGuid())
            //Perhaps remove the OrNull?
            .RuleFor(o => o.Location, f => f.PickRandom(locations).OrNull(f))
            ;

        return orderFaker.Generate(15);
    }

    internal static IEnumerable<OrderItem> GetOrderItems(List<Order> orders, List<Product> products)
    {
        var orderItemFaker = new Faker<OrderItem>()
            .UseSeed(67676)
            .RuleFor(oi => oi.Quantity, f => f.Random.Number(1, 10));
        var rng = new Random(78787);

        foreach (var order in orders)
        {
            var noOfItems = rng.Next(1, 10);
            var productsInOrder = Enumerable
                .Range(0, noOfItems)
                .Select(x => rng.Next(0, 1 + products.Count - noOfItems))
                .OrderBy(x => x)
                .Select((x, i) => products[x + i]);

            foreach (var product in productsInOrder)
            {
                var orderItem = orderItemFaker.Generate(1).First();
                orderItem.Order = order;
                orderItem.Product = product;
                yield return orderItem;
            }
        }
    }
}
