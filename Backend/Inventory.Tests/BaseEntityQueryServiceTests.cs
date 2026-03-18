using Inventory.Model.Entity;
using Inventory.Persistence.Services;
using Inventory.Services;
using Inventory.Tests.Core;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Inventory.Tests;

public class BaseEntityQueryServiceTests : BaseDatabaseTest
{
    [Test]
    public async Task AddEntity_WithSaveImmediatelyTrue_PersistsEntity()
    {
        // Arrange
        var c = CreateContext();
        
        var cat = BogusService.GetCategories(1);
        var loc = BogusService.GetLocations(1);
        var prod = BogusService.GetProducts(1, cat);
        c.AddRange(cat.First(), loc.First(), prod.First());
        await c.SaveChangesAsync();

        var li = BogusService.GetLocationItemsForEach(loc, prod).First();
        var x = new LocationItemQueryService(c);

        // Act
        await x.AddEntity(li, true);

        // Assert
        await Assert.That(c.LocationItems).Contains(li);
        
    }

    [Test]
    public async Task AddEntity_WithSaveImmediatelyFalse_DoesNotPersistUntilSaveChangesIsCalled()
    {
        // Arrange
        var c = CreateContext();

        var cat = BogusService.GetCategories(1);
        var loc = BogusService.GetLocations(1);
        var prod = BogusService.GetProducts(1, cat);
        var ord = BogusService.GetOrders(1, loc);
        c.AddRange(cat.First(), loc.First(), prod.First());
        await c.SaveChangesAsync();

        var oi = BogusService.GetOrderItems(1, ord, prod.ToList()).First();
        var x = new OrderItemQueryService(c);

        // Act
        await x.AddEntity(oi, false);

        // Assert
        await Assert.That(c.OrderItems).DoesNotContain(oi);
    }

    [Test]
    public async Task AddEntities_WithSaveImmediatelyTrue_PersistsAllEntities()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task AddEntities_WithSaveImmediatelyFalse_DoesNotPersistUntilSaveChangesIsCalled()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetEntity_WithMatchingId_ReturnsEntity()
    {
        // Arrange
        var c = CreateContext();

        var cat = BogusService.GetCategories(1);
        var prod = BogusService.GetProducts(1, cat).First();
        c.AddRange(cat.First(), prod);
        await c.SaveChangesAsync();

        var x = new ProductQueryService(c);

        // Act
        var prod2 = await x.GetEntity( new Model.Searchable.SearchableProduct() { Id = prod.Id});

        // Assert
        await Assert.That(prod2).IsEqualTo(prod);
    }

    [Test]
    public async Task GetEntity_WithoutMatch_ReturnsNull()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetEntity_WhenMultipleEntitiesMatch_ReturnsFirstOrDefault()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetEntity_WhenSearchableIdIsSet_FiltersByIdBeforeAbstractQueryArguments()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetEntityComplex_WithMatchingSearchCriteria_ReturnsEntity()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetEntityComplex_WithoutMatch_ReturnsNull()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetEntities_WithMatchingCriteria_ReturnsAllMatches()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetEntities_WithoutMatches_ReturnsEmptyCollection()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetEntities_WhenSearchableIdIsSet_FiltersByIdBeforeAbstractQueryArguments()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetEntitiesComplex_WithMatchingCriteria_ReturnsAllMatches()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetEntitiesComplex_WithoutMatches_ReturnsEmptyCollection()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetAllEntities_WhenEntitiesExist_ReturnsAllEntities()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task GetAllEntities_WhenNoEntitiesExist_ReturnsEmptyCollection()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task UpdateEntity_WithSaveImmediatelyTrue_PersistsUpdatedValues()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task UpdateEntity_WithSaveImmediatelyFalse_DoesNotPersistUntilSaveChangesIsCalled()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task UpdateEntities_WithSaveImmediatelyTrue_PersistsUpdatedValues()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task UpdateEntities_WithSaveImmediatelyFalse_DoesNotPersistUntilSaveChangesIsCalled()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task DeleteEntity_WithSaveImmediatelyTrue_RemovesEntity()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task DeleteEntity_WithSaveImmediatelyFalse_DoesNotRemoveEntityUntilSaveChangesIsCalled()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task DeleteEntityById_WithExistingId_RemovesEntity()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task DeleteEntityById_WithSaveImmediatelyFalse_DoesNotRemoveEntityUntilSaveChangesIsCalled()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task DeleteEntity_WithUnknownSearchCriteria_ThrowsExpectedExceptionOrFailsPredictably()
    {
        // Arrange

        // Act

        // Assert

    }

    [Test]
    public async Task DeleteEntityById_WithUnknownId_ThrowsExpectedExceptionOrFailsPredictably()
    {
        // Arrange

        // Act

        // Assert

    }
}
