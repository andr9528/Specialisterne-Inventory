using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Abstraction.Interfaces.Services;
using Inventory.Model.Entity;

namespace Inventory.Model.Dto;

public class CreateOrderDto
{
    public Guid Reference { get; set; } = Guid.NewGuid();
    public required IEnumerable<OrderItem> Items { get; set; }
    int? PreferredLocationId { get; set; }

    public async Task<IEnumerable<IOrder>> UnpackDto(IOrderService service)
    {
        return await service.CreateOrders(Reference, Items, PreferredLocationId);
    }
}
