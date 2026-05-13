using Inventory.Model.ComplexSearchable;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Services;
using Inventory.Services;
using Inventory.Tests.Core;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Tests;

public class LocationItemQueryServiceTests : BaseDatabaseTest
{
    [Test]
    public async Task GetEntity_WithMatchingLocationId_ReturnsLocationItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        Location expectedLocation = locations[0];
        LocationItem expected = locationItems.First(x => x.LocationId == expectedLocation.Id);

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new() {LocationId = expectedLocation.Id};

        // Act
        LocationItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.LocationId.Should().Be(expectedLocation.Id);
    }

    [Test]
    public async Task GetEntity_WithMatchingProductId_ReturnsLocationItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        Product expectedProduct = products[0];
        LocationItem expected = locationItems.First(x => x.ProductId == expectedProduct.Id);

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new() {ProductId = expectedProduct.Id};

        // Act
        LocationItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.ProductId.Should().Be(expectedProduct.Id);
    }

    [Test]
    public async Task GetEntity_WithMatchingQuantity_ReturnsLocationItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        LocationItem[] locationItems = BogusService
            .GetLocationItemsForEach(locations, products, quantityMin: 10, quantityMax: 10).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        LocationItem expected = locationItems[0];
        int quantity = expected.Quantity;

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new() {Quantity = quantity};

        // Act
        LocationItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.Quantity.Should().Be(quantity);
    }

    [Test]
    public async Task GetEntity_WithMatchingReservedQuantity_ReturnsLocationItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        LocationItem[] locationItems = BogusService
            .GetLocationItemsForEach(locations, products, reservedMin: 7, reservedMax: 7).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        LocationItem expected = locationItems[0];
        int reservedQuantity = expected.ReservedQuantity;

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new() {ReservedQuantity = reservedQuantity};

        // Act
        LocationItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.ReservedQuantity.Should().Be(reservedQuantity);
    }

    [Test]
    public async Task GetEntity_WithNonMatchingSearchCriteria_ReturnsNull()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        LocationItem[] locationItems = BogusService
            .GetLocationItemsForEach(locations, products, quantityMin: 10, quantityMax: 10).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new() {Quantity = 999};

        // Act
        LocationItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetEntities_WithMatchingLocationId_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(3, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        Location location = locations[0];
        LocationItem[] expected = locationItems.Where(x => x.LocationId == location.Id).ToArray();

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new() {LocationId = location.Id};

        // Act
        IEnumerable<LocationItem> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(expected.Length);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(expected.Select(x => x.Id));
        enumerable.Should().OnlyContain(x => x.LocationId == location.Id);
    }

    [Test]
    public async Task GetEntities_WithMatchingProductId_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(3).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        Product product = products[0];
        LocationItem[] expected = locationItems.Where(x => x.ProductId == product.Id).ToArray();

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new() {ProductId = product.Id};

        // Act
        IEnumerable<LocationItem> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(expected.Length);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(expected.Select(x => x.Id));
        enumerable.Should().OnlyContain(x => x.ProductId == product.Id);
    }

    [Test]
    public async Task GetEntities_WithMatchingQuantity_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        LocationItem[] locationItems = BogusService
            .GetLocationItemsForEach(locations, products, quantityMin: 20, quantityMax: 20).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        const int quantity = 20;

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new() {Quantity = quantity};

        // Act
        IEnumerable<LocationItem> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(locationItems.Length);
        enumerable.Should().OnlyContain(x => x.Quantity == quantity);
    }

    [Test]
    public async Task GetEntities_WithMatchingReservedQuantity_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        LocationItem[] locationItems = BogusService
            .GetLocationItemsForEach(locations, products, reservedMin: 4, reservedMax: 4).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        const int reservedQuantity = 4;

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new() {ReservedQuantity = reservedQuantity};

        // Act
        IEnumerable<LocationItem> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(locationItems.Length);
        enumerable.Should().OnlyContain(x => x.ReservedQuantity == reservedQuantity);
    }

    [Test]
    public async Task GetEntities_WithMultipleSearchCriteria_ReturnsMatchingItemsOnly()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products, quantityMin: 10,
            quantityMax: 10, reservedMin: 2, reservedMax: 2).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        Location location = locations[0];
        Product product = products[0];
        LocationItem expected = locationItems.Single(x =>
            x.LocationId == location.Id && x.ProductId == product.Id && x.Quantity == 10 && x.ReservedQuantity == 2);

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new()
        {
            LocationId = location.Id,
            ProductId = product.Id,
            Quantity = 10,
            ReservedQuantity = 2
        };

        // Act
        IEnumerable<LocationItem> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(1);
        enumerable[0].Id.Should().Be(expected.Id);
    }

    [Test]
    public async Task GetEntities_WithNoMatches_ReturnsEmptyCollection()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        LocationItem[] locationItems = BogusService
            .GetLocationItemsForEach(locations, products, quantityMin: 10, quantityMax: 10).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new() {Quantity = 12345};

        // Act
        IEnumerable<LocationItem> result = await service.GetEntities(searchable);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetEntities_WithEmptySearchable_ReturnsAllLocationItems()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(3, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new();

        // Act
        IEnumerable<LocationItem> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(locationItems.Length);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(locationItems.Select(x => x.Id));
    }

    [Test]
    public async Task GetEntityComplex_WithMinimumItemsInStock_ReturnsMatchingItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        locationItems[0].Quantity = 15;
        locationItems[1].Quantity = 10;

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        LocationItem expected = locationItems[0];

        var service = new LocationItemQueryService(context);
        ComplexSearchableLocationItem searchable = new() {MinimumItemsInStock = 10};

        // Act
        LocationItem? result = await service.GetEntityComplex(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.Quantity.Should().Be(15);
    }

    [Test]
    public async Task GetEntityComplex_WithMinimumItemsReserved_ReturnsMatchingItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(2, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        locationItems[0].ReservedQuantity = 8;
        locationItems[1].ReservedQuantity = 5;

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        LocationItem expected = locationItems[0];

        var service = new LocationItemQueryService(context);
        ComplexSearchableLocationItem searchable = new() {MinimumItemsReserved = 5};

        // Act
        LocationItem? result = await service.GetEntityComplex(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.ReservedQuantity.Should().Be(8);
    }

    [Test]
    public async Task GetEntitiesComplex_WithMinimumItemsInStock_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(3, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        locationItems[0].Quantity = 11;
        locationItems[1].Quantity = 20;
        locationItems[2].Quantity = 10;

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        LocationItem[] expected = [locationItems[0], locationItems[1]];

        var service = new LocationItemQueryService(context);
        ComplexSearchableLocationItem searchable = new() {MinimumItemsInStock = 10};

        // Act
        IEnumerable<LocationItem> result = await service.GetEntitiesComplex(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(2);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(expected.Select(x => x.Id));
        enumerable.Should().OnlyContain(x => x.Quantity > 10);
    }

    [Test]
    public async Task GetEntitiesComplex_WithMinimumItemsReserved_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(3, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        locationItems[0].ReservedQuantity = 6;
        locationItems[1].ReservedQuantity = 9;
        locationItems[2].ReservedQuantity = 5;

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        LocationItem[] expected = [locationItems[0], locationItems[1]];

        var service = new LocationItemQueryService(context);
        ComplexSearchableLocationItem searchable = new() {MinimumItemsReserved = 5};

        // Act
        IEnumerable<LocationItem> result = await service.GetEntitiesComplex(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(2);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(expected.Select(x => x.Id));
        enumerable.Should().OnlyContain(x => x.ReservedQuantity > 5);
    }

    [Test]
    public async Task GetEntitiesComplex_WithBothComplexFilters_ReturnsOnlyMatchingItems()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(3, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        locationItems[0].Quantity = 20;
        locationItems[0].ReservedQuantity = 10;

        locationItems[1].Quantity = 20;
        locationItems[1].ReservedQuantity = 1;

        locationItems[2].Quantity = 5;
        locationItems[2].ReservedQuantity = 10;

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        LocationItem expected = locationItems[0];

        var service = new LocationItemQueryService(context);
        ComplexSearchableLocationItem searchable = new()
        {
            MinimumItemsInStock = 10,
            MinimumItemsReserved = 5
        };

        // Act
        IEnumerable<LocationItem> result = await service.GetEntitiesComplex(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(1);
        enumerable[0].Id.Should().Be(expected.Id);
    }

    [Test]
    public async Task GetEntity_WhenReturnedItem_HasLocationIncluded()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        locations[0].Name = "Main Warehouse";

        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        LocationItem expected = locationItems[0];

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new()
        {
            LocationId = expected.LocationId,
            ProductId = expected.ProductId
        };

        // Act
        LocationItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Location.Should().NotBeNull();
        result.Location.Id.Should().Be(locations[0].Id);
        result.Location.Name.Should().Be("Main Warehouse");
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

        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        LocationItem expected = locationItems[0];

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new()
        {
            LocationId = expected.LocationId,
            ProductId = expected.ProductId
        };

        // Act
        LocationItem? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Product.Should().NotBeNull();
        result.Product.Id.Should().Be(products[0].Id);
        result.Product.Name.Should().Be("Gaming Mouse");
    }

    [Test]
    public async Task GetAllEntities_WhenItemsExist_ReturnsAllLocationItems()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        Product[] products = BogusService.GetProducts(3, categories).ToArray();
        LocationItem[] locationItems = BogusService.GetLocationItemsForEach(locations, products).ToArray();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.AddRange(locationItems);
        await context.SaveChangesAsync();

        var service = new LocationItemQueryService(context);

        // Act
        IEnumerable<LocationItem> result = await service.GetAllEntities();
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(locationItems.Length);
        enumerable.Select(x => x.Id).Should().BeEquivalentTo(locationItems.Select(x => x.Id));
    }

    [Test]
    public async Task GetAllEntities_WhenNoItemsExist_ReturnsEmptyCollection()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new LocationItemQueryService(context);

        // Act
        IEnumerable<LocationItem> result = await service.GetAllEntities();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task AddEntity_PersistsLocationItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        LocationItem locationItem = BogusService.GetLocationItemsForEach(locations, products).Single();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        await context.SaveChangesAsync();

        var service = new LocationItemQueryService(context);

        // Act
        await service.AddEntity(locationItem);

        // Assert
        LocationItem? persisted = await context.LocationItems.SingleOrDefaultAsync(x => x.Id == locationItem.Id);
        persisted.Should().NotBeNull();
        persisted.LocationId.Should().Be(locations[0].Id);
        persisted.ProductId.Should().Be(products[0].Id);
    }

    [Test]
    public async Task UpdateEntity_UpdatesLocationItemValues()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        LocationItem locationItem = BogusService.GetLocationItemsForEach(locations, products).Single();
        locationItem.Quantity = 10;
        locationItem.ReservedQuantity = 1;

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.Add(locationItem);
        await context.SaveChangesAsync();

        var service = new LocationItemQueryService(context);
        locationItem.Quantity = 99;
        locationItem.ReservedQuantity = 7;

        // Act
        await service.UpdateEntity(locationItem);

        // Assert
        LocationItem? updated = await context.LocationItems.SingleOrDefaultAsync(x => x.Id == locationItem.Id);
        updated.Should().NotBeNull();
        updated.Quantity.Should().Be(99);
        updated.ReservedQuantity.Should().Be(7);
    }

    [Test]
    public async Task DeleteEntity_RemovesLocationItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        LocationItem locationItem = BogusService.GetLocationItemsForEach(locations, products, quantityMin: 10,
            quantityMax: 10, reservedMin: 2, reservedMax: 2).Single();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.Add(locationItem);
        await context.SaveChangesAsync();

        var service = new LocationItemQueryService(context);
        SearchableLocationItem searchable = new()
        {
            LocationId = locationItem.LocationId,
            ProductId = locationItem.ProductId,
            Quantity = locationItem.Quantity,
            ReservedQuantity = locationItem.ReservedQuantity
        };

        // Act
        await service.DeleteEntity(searchable);

        // Assert
        LocationItem? deleted = await context.LocationItems.SingleOrDefaultAsync(x => x.Id == locationItem.Id);
        deleted.Should().BeNull();
    }

    [Test]
    public async Task DeleteEntityById_RemovesLocationItem()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(1).ToArray();
        Location[] locations = BogusService.GetLocations(1).ToArray();
        Product[] products = BogusService.GetProducts(1, categories).ToArray();
        LocationItem locationItem = BogusService.GetLocationItemsForEach(locations, products).Single();

        context.Categories.AddRange(categories);
        context.Locations.AddRange(locations);
        context.Products.AddRange(products);
        context.LocationItems.Add(locationItem);
        await context.SaveChangesAsync();

        var service = new LocationItemQueryService(context);

        // Act
        await service.DeleteEntityById(locationItem.Id);

        // Assert
        LocationItem? deleted = await context.LocationItems.SingleOrDefaultAsync(x => x.Id == locationItem.Id);
        deleted.Should().BeNull();
    }
}
