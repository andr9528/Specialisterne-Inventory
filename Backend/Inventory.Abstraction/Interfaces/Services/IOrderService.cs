using Inventory.Abstraction.Interfaces.Model.Entity;

namespace Inventory.Abstraction.Interfaces.Services;

public interface IOrderService
{
    /// <summary>
    /// Creates one or more orders for the supplied items.
    /// If a location is supplied, all items are assigned to that location.
    /// Otherwise, the fewest possible locations are selected.
    /// </summary>
    Task<IList<IOrder>> CreateOrders(Guid reference, IEnumerable<IOrderItem> items, int? locationId);
}
