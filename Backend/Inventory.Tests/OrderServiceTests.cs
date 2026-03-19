using FluentAssertions;
using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Services;
using Microsoft.Extensions.Logging;
using Moq;
using TUnit.Core;

namespace Inventory.Tests;

public class OrderServiceTests
{
    private readonly Mock<ILogger<OrderService>> loggerMock = new();
    private readonly Mock<IEntityQueryService<LocationItem, SearchableLocationItem>> locationItemQueryServiceMock = new();

    private OrderService CreateSut()
    {
        return new OrderService(loggerMock.Object, locationItemQueryServiceMock.Object);
    }

    [Test]
    public async Task CreateOrders_WithNullItems_ThrowsArgumentNullException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WithEmptyItems_ThrowsArgumentException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WithQuantityLessThanOrEqualToZero_ThrowsArgumentException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WithSingleItemAndSingleMatchingLocation_CreatesSingleOrder()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WithSingleItem_SetsReferenceIdOnCreatedOrder()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WithSingleItem_SetsLocationIdOnCreatedOrder()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WithSingleItem_SetsStatusToOpen()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WithSingleItem_AddsAllocatedProductToCreatedOrder()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WithMultipleItemsForSameLocation_CreatesSingleOrderPerLocation()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WithItemsAllocatedAcrossMultipleLocations_CreatesOneOrderPerLocation()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WhenPreferredLocationCanFulfillItem_PrefersThatLocation()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WhenPreferredLocationCannotFullyFulfillItem_AllocatesAcrossOtherLocations()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WhenItemCannotBeFullyAllocated_ThrowsInvalidOperationException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WhenAllocatedQuantityIsLowerThanRequested_ThrowsInvalidOperationException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_CallsGetEntitiesComplexForEachRequestedItem()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_CallsUpdateEntitiesWithReservedLocationItems()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WhenReservationIsApplied_DecreasesLocationItemQuantity()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WhenReservationIsApplied_IncreasesLocationItemReservedQuantity()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WhenSameLocationItemIsUsedMultipleTimes_UpdatesItOnlyOnceInUpdateEntities()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task CreateOrders_WhenLocationItemHasInsufficientQuantityDuringReservation_ThrowsInvalidOperationException()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task OrderLocationsForAllocation_WithPreferredLocation_PlacesPreferredLocationFirst()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task OrderLocationsForAllocation_WithUsedLocations_PlacesUsedLocationsBeforeUnusedLocations()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task OrderLocationsForAllocation_WithDifferentQuantities_PlacesHigherQuantityFirst()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task OrderLocationsForAllocation_WithEqualPriority_OrdersByLocationIdAscending()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task SumQuantitiesByProduct_WithSingleProduct_ReturnsSummedQuantity()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task SumQuantitiesByProduct_WithMultipleProducts_ReturnsSumPerProduct()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task SumQuantitiesByProduct_WithEmptyItems_ReturnsEmptyDictionary()
    {
        // Arrange

        // Act

        // Assert
    }
}
