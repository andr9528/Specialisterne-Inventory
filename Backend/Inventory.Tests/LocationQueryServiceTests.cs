using FluentAssertions;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Services;
using Inventory.Services;
using Inventory.Tests.Core;
using Microsoft.EntityFrameworkCore;
using TUnit.Core;

namespace Inventory.Tests;

public class LocationQueryServiceTests : BaseDatabaseTest
{
    [Test]
    public async Task GetEntity_WithMatchingName_ReturnsLocation()
    {
        // Arrange
        await using var context = CreateContext();
        Location expected = BogusService.GetLocations(1).Single();
        expected.Name = "Main Warehouse";

        context.Locations.Add(expected);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);
        SearchableLocation searchable = new() {Name = "Main Warehouse"};

        // Act
        Location? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.Name.Should().Be("Main Warehouse");
    }

    [Test]
    public async Task GetEntity_WithName_IsCaseInsensitive()
    {
        // Arrange
        await using var context = CreateContext();
        Location expected = BogusService.GetLocations(1).Single();
        expected.Name = "Main Warehouse";

        context.Locations.Add(expected);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);
        SearchableLocation searchable = new() {Name = "mAiN wArEhOuSe"};

        // Act
        Location? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.Name.Should().Be("Main Warehouse");
    }

    [Test]
    public async Task GetEntity_WithNonMatchingName_ReturnsNull()
    {
        // Arrange
        await using var context = CreateContext();
        Location location = BogusService.GetLocations(1).Single();
        location.Name = "Main Warehouse";

        context.Locations.Add(location);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);
        SearchableLocation searchable = new() {Name = "Overflow Storage"};

        // Act
        Location? result = await service.GetEntity(searchable);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetEntities_WithMatchingName_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Location[] locations = BogusService.GetLocations(3).ToArray();
        locations[0].Name = "Main Warehouse";
        locations[1].Name = "main warehouse";
        locations[2].Name = "Overflow Storage";

        context.Locations.AddRange(locations);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);
        SearchableLocation searchable = new() {Name = "MAIN WAREHOUSE"};

        // Act
        IEnumerable<Location> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(2);
        enumerable.Select(x => x.Name).Should().BeEquivalentTo("Main Warehouse", "main warehouse");
    }

    [Test]
    public async Task GetEntities_WithNoMatches_ReturnsEmptyCollection()
    {
        // Arrange
        await using var context = CreateContext();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        locations[0].Name = "Main Warehouse";
        locations[1].Name = "Overflow Storage";

        context.Locations.AddRange(locations);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);
        SearchableLocation searchable = new() {Name = "Remote Depot"};

        // Act
        IEnumerable<Location> result = await service.GetEntities(searchable);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetEntities_WithEmptySearchable_ReturnsAllLocations()
    {
        // Arrange
        await using var context = CreateContext();
        Location[] locations = BogusService.GetLocations(3).ToArray();
        locations[0].Name = "Main Warehouse";
        locations[1].Name = "Overflow Storage";
        locations[2].Name = "Front Store";

        context.Locations.AddRange(locations);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);
        SearchableLocation searchable = new();

        // Act
        IEnumerable<Location> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(3);
        enumerable.Select(x => x.Name).Should().BeEquivalentTo("Main Warehouse", "Overflow Storage", "Front Store");
    }

    [Test]
    public async Task GetAllEntities_WhenLocationsExist_ReturnsAllLocations()
    {
        // Arrange
        await using var context = CreateContext();
        Location[] locations = BogusService.GetLocations(3).ToArray();
        locations[0].Name = "Main Warehouse";
        locations[1].Name = "Overflow Storage";
        locations[2].Name = "Front Store";

        context.Locations.AddRange(locations);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);

        // Act
        IEnumerable<Location> result = await service.GetAllEntities();
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(3);
        enumerable.Select(x => x.Name).Should().BeEquivalentTo("Main Warehouse", "Overflow Storage", "Front Store");
    }

    [Test]
    public async Task GetAllEntities_WhenNoLocationsExist_ReturnsEmptyCollection()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new LocationQueryService(context);

        // Act
        IEnumerable<Location> result = await service.GetAllEntities();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task AddEntity_PersistsLocation()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new LocationQueryService(context);
        Location location = BogusService.GetLocations(1).Single();
        location.Name = "Main Warehouse";

        // Act
        await service.AddEntity(location);

        // Assert
        Location? persisted = await context.Locations.SingleOrDefaultAsync(x => x.Name == "Main Warehouse");
        persisted.Should().NotBeNull();
        persisted.Name.Should().Be("Main Warehouse");
    }

    [Test]
    public async Task UpdateEntity_UpdatesLocationName()
    {
        // Arrange
        await using var context = CreateContext();
        Location location = BogusService.GetLocations(1).Single();
        location.Name = "Main Warehouse";

        context.Locations.Add(location);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);
        location.Name = "Central Warehouse";

        // Act
        await service.UpdateEntity(location);

        // Assert
        Location? updated = await context.Locations.SingleOrDefaultAsync(x => x.Id == location.Id);
        updated.Should().NotBeNull();
        updated.Name.Should().Be("Central Warehouse");
    }

    [Test]
    public async Task DeleteEntity_RemovesLocation()
    {
        // Arrange
        await using var context = CreateContext();
        Location location = BogusService.GetLocations(1).Single();
        location.Name = "Main Warehouse";

        context.Locations.Add(location);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);

        // Act
        await service.DeleteEntity(new SearchableLocation {Name = location.Name});

        // Assert
        Location? deleted = await context.Locations.SingleOrDefaultAsync(x => x.Id == location.Id);
        deleted.Should().BeNull();
    }

    [Test]
    public async Task DeleteEntityById_RemovesLocation()
    {
        // Arrange
        await using var context = CreateContext();
        Location location = BogusService.GetLocations(1).Single();
        location.Name = "Main Warehouse";

        context.Locations.Add(location);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);

        // Act
        await service.DeleteEntityById(location.Id);

        // Assert
        Location? deleted = await context.Locations.SingleOrDefaultAsync(x => x.Id == location.Id);
        deleted.Should().BeNull();
    }

    [Test]
    public async Task GetEntity_WithWhitespaceName_DoesNotFilterByName()
    {
        // Arrange
        await using var context = CreateContext();
        Location[] locations = BogusService.GetLocations(2).ToArray();
        locations[0].Name = "Main Warehouse";
        locations[1].Name = "Overflow Storage";

        context.Locations.AddRange(locations);
        await context.SaveChangesAsync();

        var service = new LocationQueryService(context);
        SearchableLocation searchable = new() {Name = "   "};

        // Act
        Location? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(locations[0].Id);
        result.Name.Should().Be("Main Warehouse");
    }
}
