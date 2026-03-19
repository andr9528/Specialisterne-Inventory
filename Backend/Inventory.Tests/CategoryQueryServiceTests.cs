using Inventory.Tests.Core;

namespace Inventory.Tests;

public class CategoryQueryServiceTests : BaseDatabaseTest
{
    [Test]
    public async Task GetEntity_WithMatchingName_ReturnsCategory()
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
    public async Task GetAllEntities_WhenCategoriesExist_ReturnsAllCategories()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetAllEntities_WhenNoCategoriesExist_ReturnsEmptyCollection()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task AddEntity_PersistsCategory()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task UpdateEntity_UpdatesCategoryName()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task DeleteEntity_RemovesCategory()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task DeleteEntityById_RemovesCategory()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntityComplex_WithCategoryNameContains_ReturnsMatchingCategory()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntitiesComplex_WithCategoryNameContains_ReturnsAllMatches()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntitiesComplex_WithEmptyCategoryNameContains_ReturnsAllCategories()
    {
        // Arrange

        // Act

        // Assert
    }

    [Test]
    public async Task GetEntitiesComplex_WithPartialMatch_ReturnsMatchingCategories()
    {
        // Arrange

        // Act

        // Assert
    }
}
