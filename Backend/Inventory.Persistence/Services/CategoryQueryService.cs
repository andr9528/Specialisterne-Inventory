using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Core;

namespace Inventory.Persistence.Services;

public class CategoryQueryService : BaseEntityQueryService<InventoryDatabaseContext, Category, SearchableCategory>
{
    /// <inheritdoc />
    public CategoryQueryService(InventoryDatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    protected override IQueryable<Category> AddComplexQueryArguments(IQueryable<Category> basicQuery, IComplexSearchable<SearchableCategory> complex)
    {
        // No implementation of `IComplexSearchable<SearchableCategory>` exist - Throwing.
        throw new NotImplementedException();
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
