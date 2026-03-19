using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.ComplexSearchable;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Core;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Services;

public class CategoryQueryService : BaseEntityQueryService<InventoryDatabaseContext, Category, SearchableCategory>
{
    /// <inheritdoc />
    public CategoryQueryService(InventoryDatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    protected override IQueryable<Category> AddComplexQueryArguments(IQueryable<Category> query, IComplexSearchable<SearchableCategory> complex)
    {
        if (complex is not ComplexSearchableCategory complexSearchableProduct)
        {
            throw new ArgumentException(
                $"Expected {nameof(complex)} to be of type {nameof(ComplexSearchableCategory)}, but it wasn't.");
        }

        if (!string.IsNullOrWhiteSpace(complexSearchableProduct.CategoryNameContains))
        {
            string keyword = $"%{complexSearchableProduct.CategoryNameContains}%";
            query = query.Where(x => EF.Functions.Like(x.Name, keyword));
        }

        return query;
    }

    /// <inheritdoc />
    protected override IEnumerable<Category> ApplyComplexNonDatabaseQueryArguments(IEnumerable<Category> entities, IComplexSearchable<SearchableCategory> complex)
    {
        return entities;
    }

    /// <inheritdoc />
    protected override IQueryable<Category> GetBaseQuery()
    {
        return context.Categories.AsQueryable();
    }

    /// <inheritdoc />
    protected override IQueryable<Category> AddQueryArguments(SearchableCategory searchable, IQueryable<Category> query)
    {
        if (!string.IsNullOrWhiteSpace(searchable.Name))
        {
            query = query.Where(x => x.Name.ToLower() == searchable.Name.ToLower());
        }

        return query;
    }
}
