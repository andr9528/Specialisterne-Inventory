using Bogus;
using Inventory.Abstraction.Enum;
using Inventory.Model.Entity;

namespace Inventory.Services;

public class BogusService
{
    public static IEnumerable<Category> GetCategories(int noOfCategories)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(noOfCategories, 1);
        var categoryFaker = new Faker<Category>()
            .UseSeed(12121)
            .RuleFor(c => c.Name, f => f.Commerce.Department());
        return categoryFaker.Generate(noOfCategories);
    }

    public static IEnumerable<Location> GetLocations(int noOfLocations)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(noOfLocations, 1);
        var locationFaker = new Faker<Location>()
            .UseSeed(23232)
            .RuleFor(l => l.Name, f => f.Company.CompanyName());
        return locationFaker.Generate(noOfLocations);
    }

    public static IEnumerable<Product> GetProducts(int noOfProducts, IEnumerable<Category> categories)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(noOfProducts, 1);
        if (!categories.Any())
            throw new ArgumentException($"{nameof(categories)} argument must have at least one member");
        var productFaker = new Faker<Product>()
            .UseSeed(34343)
            .RuleFor(p => p.Category, f => f.PickRandom(categories))
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price()))
            ;
        return productFaker.Generate(noOfProducts);
    }

    public static IEnumerable<LocationItem> GetLocationItemsForEach(IEnumerable<Location> locations, IEnumerable<Product> products,
        int quantityMin = 0, int quantityMax = 1000, int targetMin = 100, int targetMax = 500, int reservedMin = 0, int reservedMax = 50)
    {
        if (!locations.Any())
            throw new ArgumentException($"{nameof(locations)} argument must have at least one member");
        if (!products.Any())
            throw new ArgumentException($"{nameof(products)} argument must have at least one member");
        var locationItemFaker = new Faker<LocationItem>()
            .UseSeed(45454)
            .RuleFor(li => li.Quantity, f => f.Random.Number(quantityMin, quantityMax))
            .RuleFor(li => li.TargetQuantity, f => f.Random.Number(targetMin, targetMax))
            .RuleFor(li => li.ReservedQuantity, f => f.Random.Number(reservedMin, reservedMax));
        //Need to make sure locations is iterated, otherwise EF gets mad
        foreach (var location in locations.ToList())
        {
            foreach (var product in products)
            {
                var locationItem = locationItemFaker.Generate(1).First();
                locationItem.Location = location;
                locationItem.Product = product;
                yield return locationItem;
            }
        }
    }

    public static IEnumerable<Order> GetOrders(int noOfOrders, IEnumerable<Location> locations)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(noOfOrders, 1);
        if (!locations.Any())
            throw new ArgumentException($"{nameof(locations)} argument must have at least one member");
        var orderFaker = new Faker<Order>()
            .UseSeed(56565)
            .RuleFor(o => o.Status, f => f.PickRandom<OrderStatus>())
            .RuleFor(o => o.ReferenceId, f => Guid.NewGuid())
            //Perhaps remove the OrNull?
            .RuleFor(o => o.Location, f => f.PickRandom(locations).OrNull(f))
            ;

        return orderFaker.Generate(noOfOrders);
    }


    public static IEnumerable<OrderItem> GetOrderItems(int maxItemsPerOrder, IEnumerable<Order> orders, IList<Product> products)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxItemsPerOrder, 1);
        if (!orders.Any())
            throw new ArgumentException($"{nameof(orders)} argument must have at least one member");
        if (!products.Any())
            throw new ArgumentException($"{nameof(products)} argument must have at least one member");
        var orderItemFaker = new Faker<OrderItem>()
            .UseSeed(67676)
            .RuleFor(oi => oi.Quantity, f => f.Random.Number(1, 10));
        var rng = new Random(78787);
        foreach (var order in orders)
        {
            var noOfItems = rng.Next(1, maxItemsPerOrder);
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
