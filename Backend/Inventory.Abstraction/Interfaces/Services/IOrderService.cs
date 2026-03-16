using Inventory.Abstraction.Interfaces.Model.Entity;

namespace Inventory.Abstraction.Interfaces.Services;

public interface IOrderService
{
    /// <summary>
    /// Creates one or more orders for the supplied items.
    /// If a location id is supplied, then that location will be preferred for allocation.
    /// Tries to limit the amount of locations used and orders generated.
    /// </summary>
    Task<IList<IOrder>> CreateOrders(Guid reference, IEnumerable<IOrderItem> items, int? locationId);
}
