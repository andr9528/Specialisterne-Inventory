using Inventory.Model.ComplexSearchable;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Services;
using Inventory.Services;
using Inventory.Tests.Core;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Tests;

public class CategoryQueryServiceTests : BaseDatabaseTest
{
    [Test]
    public async Task GetEntity_WithMatchingName_ReturnsCategory()
    {
        // Arrange
        await using var context = CreateContext();
        Category expected = BogusService.GetCategories(1).Single();
        expected.Name = "Electronics";

        context.Categories.Add(expected);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);
        SearchableCategory searchable = new() {Name = "Electronics"};

        // Act
        Category? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.Name.Should().Be("Electronics");
    }

    [Test]
    public async Task GetEntity_WithName_IsCaseInsensitive()
    {
        // Arrange
        await using var context = CreateContext();
        Category expected = BogusService.GetCategories(1).Single();
        expected.Name = "Electronics";

        context.Categories.Add(expected);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);
        SearchableCategory searchable = new() {Name = "eLeCtRoNiCs"};

        // Act
        Category? result = await service.GetEntity(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(expected.Id);
        result.Name.Should().Be("Electronics");
    }

    [Test]
    public async Task GetEntity_WithNonMatchingName_ReturnsNull()
    {
        // Arrange
        await using var context = CreateContext();
        Category category = BogusService.GetCategories(1).Single();
        category.Name = "Electronics";

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);
        SearchableCategory searchable = new() {Name = "Furniture"};

        // Act
        Category? result = await service.GetEntity(searchable);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task GetEntities_WithMatchingName_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(3).ToArray();
        categories[0].Name = "Electronics";
        categories[1].Name = "electronics";
        categories[2].Name = "Furniture";

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);
        SearchableCategory searchable = new() {Name = "ELECTRONICS"};

        // Act
        IEnumerable<Category> result = await service.GetEntities(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(2);
        enumerable.Select(x => x.Name).Should().BeEquivalentTo("Electronics", "electronics");
    }

    [Test]
    public async Task GetEntities_WithNoMatches_ReturnsEmptyCollection()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(2).ToArray();
        categories[0].Name = "Electronics";
        categories[1].Name = "Furniture";

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);
        SearchableCategory searchable = new() {Name = "Books"};

        // Act
        IEnumerable<Category> result = await service.GetEntities(searchable);

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetAllEntities_WhenCategoriesExist_ReturnsAllCategories()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(3).ToArray();
        categories[0].Name = "Electronics";
        categories[1].Name = "Furniture";
        categories[2].Name = "Books";

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);

        // Act
        IEnumerable<Category> result = await service.GetAllEntities();
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(3);
        enumerable.Select(x => x.Name).Should().BeEquivalentTo("Electronics", "Furniture", "Books");
    }

    [Test]
    public async Task GetAllEntities_WhenNoCategoriesExist_ReturnsEmptyCollection()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new CategoryQueryService(context);

        // Act
        IEnumerable<Category> result = await service.GetAllEntities();

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task AddEntity_PersistsCategory()
    {
        // Arrange
        await using var context = CreateContext();
        var service = new CategoryQueryService(context);
        Category category = BogusService.GetCategories(1).Single();
        category.Name = "Electronics";

        // Act
        await service.AddEntity(category);

        // Assert
        Category? persisted = await context.Categories.SingleOrDefaultAsync(x => x.Name == "Electronics");
        persisted.Should().NotBeNull();
        persisted.Name.Should().Be("Electronics");
    }

    [Test]
    public async Task UpdateEntity_UpdatesCategoryName()
    {
        // Arrange
        await using var context = CreateContext();
        Category category = BogusService.GetCategories(1).Single();
        category.Name = "Electronics";

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);
        category.Name = "Home Electronics";

        // Act
        await service.UpdateEntity(category);

        // Assert
        Category? updated = await context.Categories.SingleOrDefaultAsync(x => x.Id == category.Id);
        updated.Should().NotBeNull();
        updated.Name.Should().Be("Home Electronics");
    }

    [Test]
    public async Task DeleteEntity_RemovesCategory()
    {
        // Arrange
        await using var context = CreateContext();
        Category category = BogusService.GetCategories(1).Single();
        category.Name = "Electronics";

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);

        // Act
        await service.DeleteEntity(new SearchableCategory(){Name = category.Name});

        // Assert
        Category? deleted = await context.Categories.SingleOrDefaultAsync(x => x.Id == category.Id);
        deleted.Should().BeNull();
    }

    [Test]
    public async Task DeleteEntityById_RemovesCategory()
    {
        // Arrange
        await using var context = CreateContext();
        Category category = BogusService.GetCategories(1).Single();
        category.Name = "Electronics";

        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);

        // Act
        await service.DeleteEntityById(category.Id);

        // Assert
        Category? deleted = await context.Categories.SingleOrDefaultAsync(x => x.Id == category.Id);
        deleted.Should().BeNull();
    }

    [Test]
    public async Task GetEntityComplex_WithCategoryNameContains_ReturnsMatchingCategory()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(2).ToArray();
        categories[0].Name = "Gaming Electronics";
        categories[1].Name = "Furniture";

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);
        ComplexSearchableCategory searchable = new() {CategoryNameContains = "Elect"};

        // Act
        Category? result = await service.GetEntityComplex(searchable);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(categories[0].Id);
        result.Name.Should().Be("Gaming Electronics");
    }

    [Test]
    public async Task GetEntitiesComplex_WithCategoryNameContains_ReturnsAllMatches()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(3).ToArray();
        categories[0].Name = "Electronics";
        categories[1].Name = "Home Electronics";
        categories[2].Name = "Furniture";

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);
        ComplexSearchableCategory searchable = new() {CategoryNameContains = "Elect"};

        // Act
        IEnumerable<Category> result = await service.GetEntitiesComplex(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(2);
        enumerable.Select(x => x.Name).Should().BeEquivalentTo("Electronics", "Home Electronics");
    }

    [Test]
    public async Task GetEntitiesComplex_WithEmptyCategoryNameContains_ReturnsAllCategories()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(3).ToArray();
        categories[0].Name = "Electronics";
        categories[1].Name = "Furniture";
        categories[2].Name = "Books";

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);
        ComplexSearchableCategory searchable = new() {CategoryNameContains = string.Empty};

        // Act
        IEnumerable<Category> result = await service.GetEntitiesComplex(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(3);
        enumerable.Select(x => x.Name).Should().BeEquivalentTo("Electronics", "Furniture", "Books");
    }

    [Test]
    public async Task GetEntitiesComplex_WithPartialMatch_ReturnsMatchingCategories()
    {
        // Arrange
        await using var context = CreateContext();
        Category[] categories = BogusService.GetCategories(3).ToArray();
        categories[0].Name = "Office Supplies";
        categories[1].Name = "Office Furniture";
        categories[2].Name = "Electronics";

        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        var service = new CategoryQueryService(context);
        ComplexSearchableCategory searchable = new() {CategoryNameContains = "Office"};

        // Act
        IEnumerable<Category> result = await service.GetEntitiesComplex(searchable);
        var enumerable = result.ToList();

        // Assert
        enumerable.Should().HaveCount(2);
        enumerable.Select(x => x.Name).Should().BeEquivalentTo("Office Supplies", "Office Furniture");
    }
}
