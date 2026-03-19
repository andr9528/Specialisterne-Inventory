using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Core;

namespace Inventory.Persistence.Services;

public class OrderQueryService : BaseEntityQueryService<InventoryDatabaseContext, Order, SearchableOrder>
{
    /// <inheritdoc />
    public OrderQueryService(InventoryDatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    protected override IQueryable<Order> AddComplexQueryArguments(IQueryable<Order> query, IComplexSearchable<SearchableOrder> complex)
    {
        // No implementation of `IComplexSearchable<SearchableOrder>` exist - Throwing.
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override IEnumerable<Order> ApplyComplexNonDatabaseQueryArguments(IEnumerable<Order> entities, IComplexSearchable<SearchableOrder> complex)
    {
        return entities;
    }

    /// <inheritdoc />
    protected override IQueryable<Order> GetBaseQuery()
    {
        return context.Orders.AsQueryable();
    }

    /// <inheritdoc />
    protected override IQueryable<Order> AddQueryArguments(SearchableOrder searchable, IQueryable<Order> query)
    {
        if (!Equals(searchable.Status, default(OrderStatus)))
        {
            query = query.Where(x => x.Status == searchable.Status);
        }

        if (!Equals(searchable.ReferenceId, Guid.Empty))
        {
            query = query.Where(x => x.ReferenceId == searchable.ReferenceId);
        }

        if (searchable.LocationId != 0)
        {
            query = query.Where(x => x.LocationId == searchable.LocationId);
        }

        return query;
    }
}
