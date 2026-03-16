using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Core;

namespace Inventory.Persistence.Services;

public class LocationQueryService : BaseEntityQueryService<InventoryDatabaseContext, Location, SearchableLocation>
{
    /// <inheritdoc />
    public LocationQueryService(InventoryDatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    protected override IQueryable<Location> AddComplexQueryArguments(IQueryable<Location> query, IComplexSearchable<SearchableLocation> complex)
    {
        // No implementation of `IComplexSearchable<SearchableLocation>` exist - Throwing.
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override IQueryable<Location> GetBaseQuery()
    {
        return context.Locations.AsQueryable();
    }

    /// <inheritdoc />
    protected override IQueryable<Location> AddQueryArguments(SearchableLocation searchable, IQueryable<Location> query)
    {
        if (!string.IsNullOrWhiteSpace(searchable.Name))
        {
            query = query.Where(x => x.Name.ToLower() == searchable.Name.ToLower());
        }

        return query;
    }
}
