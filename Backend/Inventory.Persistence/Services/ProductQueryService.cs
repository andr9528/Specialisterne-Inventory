using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Core;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Services;

public class ProductQueryService : BaseEntityQueryService<InventoryDatabaseContext, Product, SearchableProduct>
{
    /// <inheritdoc />
    public ProductQueryService(InventoryDatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    protected override IQueryable<Product> AddComplexQueryArguments(IQueryable<Product> query, IComplexSearchable<SearchableProduct> complex)
    {
        // No implementation of `IComplexSearchable<SearchableProduct>` exist - Throwing.
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override IQueryable<Product> GetBaseQuery()
    {
        return context.Products.AsQueryable().Include(x => x.Category);
    }

    /// <inheritdoc />
    protected override IQueryable<Product> AddQueryArguments(SearchableProduct searchable, IQueryable<Product> query)
    {
        if (searchable.Price != 0)
        {
            query = query.Where(x => x.Price == searchable.Price);
        }

        if (!string.IsNullOrWhiteSpace(searchable.Name))
        {
            query = query.Where(x => x.Name.ToLower() == searchable.Name.ToLower());
        }

        return query;
    }
}
