using FluentAssertions;
using Inventory.Persistence.Services;
using Inventory.Tests.Core;
using TUnit.Core;

namespace Inventory.Tests;

public class OrderItemQueryServiceTests : BaseDatabaseTest
{
    [Test]
    public async Task GetEntity_WithMatchingOrderId_ReturnsOrderItem()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntity_WithMatchingProductId_ReturnsOrderItem()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntity_WithMatchingOrderIdAndProductId_ReturnsOrderItem()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntity_WithNonMatchingSearchCriteria_ReturnsNull()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntities_WithMatchingOrderId_ReturnsAllMatches()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntities_WithMatchingProductId_ReturnsAllMatches()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntities_WithMatchingOrderIdAndProductId_ReturnsAllMatches()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntities_WithNoMatches_ReturnsEmptyCollection()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntities_WithEmptySearchable_ReturnsAllOrderItems()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntity_WhenReturnedItem_HasOrderIncluded()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntity_WhenReturnedItem_HasProductIncluded()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetAllEntities_WhenOrderItemsExist_ReturnsAllOrderItems()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetAllEntities_WhenNoOrderItemsExist_ReturnsEmptyCollection()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task AddEntity_PersistsOrderItem()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task UpdateEntity_UpdatesOrderItemValues()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task DeleteEntity_RemovesOrderItem()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task DeleteEntityById_RemovesOrderItem()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntityComplex_ThrowsNotImplementedException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntitiesComplex_ThrowsNotImplementedException()
    {
        // Arrange

        // Act

        // Assert
    }
}
