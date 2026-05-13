using FluentAssertions;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Services;
using Inventory.Services;
using Inventory.Tests.Core;
using Microsoft.EntityFrameworkCore;
using TUnit.Core;

namespace Inventory.Tests;

public class OrderItemQueryServiceTests : BaseDatabaseTest
{
    [Test]
    public async Task GetEntity_WithMatchingOrderId_ReturnsOrderItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        Order[] orders = BogusService.GetOrders(2, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(2, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        Order expectedOrder = orders[0];
        OrderItem expected = orderItems.First(x => x.OrderId == expectedOrder.Id);

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new() {OrderId = expectedOrder.Id};

        // Act
        OrderItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.OrderId.Should().Be(expectedOrder.Id);
    }

    [Test]
    public async Task GetEntity_WithMatchingProductId_ReturnsOrderItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        Order[] orders = BogusService.GetOrders(2, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(2, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        Product expectedProduct = products[0];
        OrderItem expected = orderItems.First(x => x.ProductId == expectedProduct.Id);

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new() {ProductId = expectedProduct.Id};

        // Act
        OrderItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.ProductId.Should().Be(expectedProduct.Id);
    }

    [Test]
    public async Task GetEntity_WithMatchingOrderIdAndProductId_ReturnsOrderItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        Order[] orders = BogusService.GetOrders(2, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(2, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        OrderItem expected = orderItems[0];

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new()
        {
            OrderId = expected.OrderId,
            ProductId = expected.ProductId
        };

        // Act
        OrderItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.OrderId.Should().Be(expected.OrderId);
        result.ProductId.Should().Be(expected.ProductId);
    }

    [Test]
    public async Task GetEntity_WithNonMatchingSearchCriteria_ReturnsNull()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        Order[] orders = BogusService.GetOrders(1, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(2, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new() {OrderId = int.MaxValue};

        // Act
        OrderItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetEntities_WithMatchingOrderId_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(3, categories).ToArray();
        Order[] orders = BogusService.GetOrders(2, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(3, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        Order expectedOrder = orders[0];
        OrderItem[] expected = orderItems.Where(x => x.OrderId == expectedOrder.Id).ToArray();

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new() {OrderId = expectedOrder.Id};

        // Act
        IEnumerable<OrderItem> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(expected.Length);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(expected.Select(x => x.Id));
        enumerable.Should().OnlyContain(x => x.OrderId == expectedOrder.Id);
    }

    [Test]
    public async Task GetEntities_WithMatchingProductId_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(3).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        Order[] orders = BogusService.GetOrders(3, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(2, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        Product expectedProduct = products[0];
        OrderItem[] expected = orderItems.Where(x => x.ProductId == expectedProduct.Id).ToArray();

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new() {ProductId = expectedProduct.Id};

        // Act
        IEnumerable<OrderItem> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(expected.Length);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(expected.Select(x => x.Id));
        enumerable.Should().OnlyContain(x => x.ProductId == expectedProduct.Id);
    }

    [Test]
    public async Task GetEntities_WithMatchingOrderIdAndProductId_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(3, categories).ToArray();
        Order[] orders = BogusService.GetOrders(2, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(3, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        OrderItem expected = orderItems[0];
        OrderItem[] expectedMatches = orderItems
            .Where(x => x.OrderId == expected.OrderId && x.ProductId == expected.ProductId).ToArray();

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new()
        {
            OrderId = expected.OrderId,
            ProductId = expected.ProductId
        };

        // Act
        IEnumerable<OrderItem> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(expectedMatches.Length);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(expectedMatches.Select(x => x.Id));
        enumerable.Should().OnlyContain(x => x.OrderId == expected.OrderId && x.ProductId == expected.ProductId);
    }

    [Test]
    public async Task GetEntities_WithNoMatches_ReturnsEmptyCollection()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        Order[] orders = BogusService.GetOrders(1, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(2, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new() {ProductId = int.MaxValue};

        // Act
        IEnumerable<OrderItem> result = await service.GetEntities(searchable);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetEntities_WithEmptySearchable_ReturnsAllOrderItems()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(3, categories).ToArray();
        Order[] orders = BogusService.GetOrders(2, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(3, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new();

        // Act
        IEnumerable<OrderItem> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(orderItems.Length);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(orderItems.Select(x => x.Id));
    }

    [Test]
    public async Task GetEntity_WhenReturnedItem_HasOrderIncluded()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        Order[] orders = BogusService.GetOrders(1, locations).ToArray();
        orders[0].ReferenceId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        OrderItem[] orderItems = BogusService.GetOrderItems(2, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        OrderItem expected = orderItems[0];

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new()
        {
            OrderId = expected.OrderId,
            ProductId = expected.ProductId
        };

        // Act
        OrderItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Order.Should().NotBeNull();
        result.Order.Id.Should().Be(orders[0].Id);
        result.Order.ReferenceId.Should().Be(Guid.Parse("11111111-1111-1111-1111-111111111111"));
    }

    [Test]
    public async Task GetEntity_WhenReturnedItem_HasProductIncluded()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        categories[0].Name = "Electronics";

        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        products[0].Name = "Gaming Mouse";

        Order[] orders = BogusService.GetOrders(1, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(2, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        OrderItem expected = orderItems[0];

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new()
        {
            OrderId = expected.OrderId,
            ProductId = expected.ProductId
        };

        // Act
        OrderItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Product.Should().NotBeNull();
        result.Product.Id.Should().Be(products[0].Id);
        result.Product.Name.Should().Be("Gaming Mouse");
    }

    [Test]
    public async Task GetAllEntities_WhenOrderItemsExist_ReturnsAllOrderItems()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(3, categories).ToArray();
        Order[] orders = BogusService.GetOrders(2, locations).ToArray();
        OrderItem[] orderItems = BogusService.GetOrderItems(3, orders, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.AddRange(orderItems);
        await context.SaveChangesAsync();

        var service = new OrderItemQueryService(context);

        // Act
        IEnumerable<OrderItem> result = await service.GetAllEntities();
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(orderItems.Length);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(orderItems.Select(x => x.Id));
    }

    [Test]
    public async Task GetAllEntities_WhenNoOrderItemsExist_ReturnsEmptyCollection()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new OrderItemQueryService(context);

        // Act
        IEnumerable<OrderItem> result = await service.GetAllEntities();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task AddEntity_PersistsOrderItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        Order[] orders = BogusService.GetOrders(1, locations).ToArray();
        OrderItem orderItem = BogusService.GetOrderItems(2, orders, products).Single();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        await context.SaveChangesAsync();

        var service = new OrderItemQueryService(context);

        // Act
        await service.AddEntity(orderItem);

        // Assert
        OrderItem? persisted = await context.OrderItems.SingleOrDefaultAsync(x => x.Id == orderItem.Id);
        persisted.Should().NotBeNull();
        persisted.OrderId.Should().Be(orders[0].Id);
        persisted.ProductId.Should().Be(products[0].Id);
    }

    [Test]
    public async Task UpdateEntity_UpdatesOrderItemValues()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        Order[] orders = BogusService.GetOrders(1, locations).ToArray();
        OrderItem orderItem = BogusService.GetOrderItems(2, orders, products).Single();
        orderItem.Quantity = 2;

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.Add(orderItem);
        await context.SaveChangesAsync();

        var service = new OrderItemQueryService(context);
        orderItem.Quantity = 9;

        // Act
        await service.UpdateEntity(orderItem);

        // Assert
        OrderItem? updated = await context.OrderItems.SingleOrDefaultAsync(x => x.Id == orderItem.Id);
        updated.Should().NotBeNull();
        updated.Quantity.Should().Be(9);
    }

    [Test]
    public async Task DeleteEntity_RemovesOrderItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        Order[] orders = BogusService.GetOrders(1, locations).ToArray();
        OrderItem orderItem = BogusService.GetOrderItems(2, orders, products).Single();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.Add(orderItem);
        await context.SaveChangesAsync();

        var service = new OrderItemQueryService(context);
        SearchableOrderItem searchable = new()
        {
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId
        };

        // Act
        await service.DeleteEntity(searchable);

        // Assert
        OrderItem? deleted = await context.OrderItems.SingleOrDefaultAsync(x => x.Id == orderItem.Id);
        deleted.Should().BeNull();
    }

    [Test]
    public async Task DeleteEntityById_RemovesOrderItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        Order[] orders = BogusService.GetOrders(1, locations).ToArray();
        OrderItem orderItem = BogusService.GetOrderItems(2, orders, products).Single();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.Orders.AddRange(orders);
        context.OrderItems.Add(orderItem);
        await context.SaveChangesAsync();

        var service = new OrderItemQueryService(context);

        // Act
        await service.DeleteEntityById(orderItem.Id);

        // Assert
        OrderItem? deleted = await context.OrderItems.SingleOrDefaultAsync(x => x.Id == orderItem.Id);
        deleted.Should().BeNull();
    }
}
