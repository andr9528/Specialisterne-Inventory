using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Entity;
using Inventory.Model.Searchable;
using Inventory.Persistence.Core;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Persistence.Services;

public class OrderItemQueryService : BaseEntityQueryService<InventoryDatabaseContext, OrderItem, SearchableOrderItem>
{
    /// <inheritdoc />
    public OrderItemQueryService(InventoryDatabaseContext context) : base(context)
    {
    }

    /// <inheritdoc />
    protected override IQueryable<OrderItem> AddComplexQueryArguments(IQueryable<OrderItem> basicQuery, IComplexSearchable<SearchableOrderItem> complex)
    {
        // No implementation of `IComplexSearchable<SearchableOrderItem>` exist - Throwing.
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override IQueryable<OrderItem> GetBaseQuery()
    {
        return context.OrderItems.AsQueryable().Include(x => x.Order).Include(x => x.Product);
    }

    /// <inheritdoc />
    protected override IQueryable<OrderItem> AddQueryArguments(SearchableOrderItem searchable, IQueryable<OrderItem> query)
    {
        if (searchable.OrderId != 0)
        {
            query = query.Where(x => x.OrderId == searchable.OrderId);
        }

        if (searchable.ProductId != 0)
        {
            query = query.Where(x => x.ProductId == searchable.ProductId);
        }

        return query;
    }
}
