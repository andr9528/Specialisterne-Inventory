using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.ComplexSearchable;
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
    protected override IQueryable<LocationItem> AddComplexQueryArguments(IQueryable<LocationItem> query, IComplexSearchable<SearchableLocationItem> complex)
    {
        if (complex is not ComplexSearchableLocationItem complexSearchableLocationItem)
        {
            throw new ArgumentException($"Expected {nameof(complex)} to be of type {nameof(ComplexSearchableLocationItem)}, but it wasn't.");
        }

        if (complexSearchableLocationItem.MinimumItemsInStock.HasValue)
        {
            query = query.Where(x => x.Quantity > complexSearchableLocationItem.MinimumItemsInStock);
        }

        if (complexSearchableLocationItem.MinimumItemsReserved.HasValue)
        {
            query = query.Where(x => x.ReservedQuantity > complexSearchableLocationItem.MinimumItemsReserved);
        }

        return query;
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

        if (searchable.ReservedQuantity != 0)
        {
            query = query.Where(x => x.ReservedQuantity == searchable.ReservedQuantity);
        }

        return query;
    }
}
