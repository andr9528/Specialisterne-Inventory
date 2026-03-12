using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Core;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Services;

public class LocationItemQueryService : BaseEntityQueryService<InventoryDatabaseContext, LocationItem, SearchableLocationItem>
{
    /// <inheritdoc />
    public LocationItemQueryService(InventoryDatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    protected override IQueryable<LocationItem> AddComplexQueryArguments(IQueryable<LocationItem> basicQuery, IComplexSearchable<SearchableLocationItem> complex)
    {
        // No implementation of `IComplexSearchable<SearchableLocationItem>` exist - Throwing.
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override IQueryable<LocationItem> GetBaseQuery()
    {
        return context.LocationItems.AsQueryable().Include(x=> x.Location).Include(x=> x.Product);
    }

    /// <inheritdoc />
    protected override IQueryable<LocationItem> AddQueryArguments(SearchableLocationItem searchable, IQueryable<LocationItem> query)
    {
        if (searchable.LocationId != 0)
        {
            query = query.Where(x => x.LocationId == searchable.LocationId);
        }

        if (searchable.ProductId != 0)
        {
            query = query.Where(x => x.ProductId == searchable.ProductId);
        }

        if (searchable.Quantity != 0)
        {
            query = query.Where(x => x.Quantity == searchable.Quantity);
        }

        return query;
    }
}
