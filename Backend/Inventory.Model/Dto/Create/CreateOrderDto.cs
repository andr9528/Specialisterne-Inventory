using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Abstraction.Interfaces.Services;
using Inventory.Model.Entity;

namespace Inventory.Model.Dto.Create;

public class CreateOrderDto
{
    public Guid Reference { get; set; } = Guid.NewGuid();
    public required IEnumerable<CreateOrderItemDto> Items { get; set; }
    int? PreferredLocationId { get; set; }

    public async Task<IList<IOrder>> UnpackDto(IOrderService service)
    {
        return await service.CreateOrders(Reference, Items.Select(x => x.UnpackDto()), PreferredLocationId);
    }
}
