using FluentAssertions;
using Inventory.Persistence.Services;
using Inventory.Tests.Core;
using TUnit.Core;

namespace Inventory.Tests;

public class LocationQueryServiceTests : BaseDatabaseTest
{
    [Test]
    public async Task GetEntity_WithMatchingName_ReturnsLocation()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntity_WithName_IsCaseInsensitive()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntity_WithNonMatchingName_ReturnsNull()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntities_WithMatchingName_ReturnsAllMatches()
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
    public async Task GetEntities_WithEmptySearchable_ReturnsAllLocations()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetAllEntities_WhenLocationsExist_ReturnsAllLocations()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetAllEntities_WhenNoLocationsExist_ReturnsEmptyCollection()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task AddEntity_PersistsLocation()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task UpdateEntity_UpdatesLocationName()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task DeleteEntity_RemovesLocation()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task DeleteEntityById_RemovesLocation()
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
