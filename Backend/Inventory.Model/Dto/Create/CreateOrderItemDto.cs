using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Model.Entity;

namespace Inventory.Model.Dto.Create;

public class CreateOrderItemDto
{
    public required int ProductId { get; set; }
    public required int Quantity { get; set; }

    public IOrderItem UnpackDto()
    {
        return new OrderItem() { ProductId = ProductId, Quantity = Quantity };
    }
}
