using System.Security.Cryptography.X509Certificates;
using Bogus;
using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Startup;
using Inventory.Model.Entity;
using Inventory.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Server.Startup;

public class BogusStartupModule : IServiceStartupModule
{
    public void ConfigureServices(IServiceCollection services)
    {
        ServiceProvider? provider = services.BuildServiceProvider();
        FakeData.Init();
        using (var context = provider.GetService<InventoryDatabaseContext>())
        {
            context.Categories.AddRange(FakeData.Categories);
            context.Locations.AddRange(FakeData.Locations);
            context.Products.AddRange(FakeData.Products);
            context.LocationItems.AddRange(FakeData.LocationItems);
            context.Orders.AddRange(FakeData.Orders);
            context.OrderItems.AddRange(FakeData.OrderItems);

            //context.SaveChanges();
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

    public static void Init()
    {
        var productFaker = new Faker<Product>()
            //.StrictMode(true)
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
            ;

        var categoryFaker = new Faker<Category>()
            .RuleFor(c => c.Id, f => f.IndexFaker + 1)
            .RuleFor(c => c.Name, f => f.Commerce.Department())
            .RuleFor(c => c.Products, (f, c) =>
            {
                productFaker.RuleFor(p => p.CategoryId, _ => c.Id);
                var products = productFaker.GenerateBetween(3, 5);
                FakeData.Products.AddRange(products);

                return null;
            });
        FakeData.Categories.AddRange(categoryFaker.Generate(5));

        var locationItemFaker = new Faker<LocationItem>()
            .RuleFor(li => li.Id, f => f.IndexFaker + 1)
            .RuleFor(li => li.ProductId, f=> f.PickRandom( FakeData.Products).Id);
            


        var locationFaker = new Faker<Location>()
            .RuleFor(l => l.Id, f => f.IndexFaker + 1)
            .RuleFor(l => l.Name, f => f.Company.CompanyName())
            .RuleFor(l => l.Products, (f, l) =>
            {
                locationItemFaker.RuleFor(li => li.LocationId, _ => l.Id);
                var locationItems = locationItemFaker.GenerateBetween(5, 10);
                FakeData.LocationItems.AddRange(locationItems);
                
                return null;
            });

        FakeData.Locations.AddRange(locationFaker.Generate(5));


        var orderItemFaker = new Faker<OrderItem>()
            .RuleFor(o => o.Id, f => f.IndexFaker + 1)
            .RuleFor(li => li.ProductId, f => f.PickRandom(FakeData.Products).Id);

        var orderFaker = new Faker<Order>()
            .RuleFor(o => o.Id, f => f.IndexFaker + 1)
            .RuleFor(o => o.Status, f => f.PickRandom<OrderStatus>())
            .RuleFor(o => o.ReferenceId, f => Guid.NewGuid())
            .RuleFor(o => o.LocationId, f=> f.PickRandom(FakeData.Locations).Id)
            .RuleFor(o => o.Products, (f, o) =>
            {
                orderItemFaker.RuleFor(oi => oi.OrderId, _ => o.Id);
                var orderItems = orderItemFaker.GenerateBetween(1, 5);
                FakeData.OrderItems.AddRange(orderItems);

                return null;
            });
        FakeData.Orders.AddRange(orderFaker.Generate(5));
        
        
    }
}
