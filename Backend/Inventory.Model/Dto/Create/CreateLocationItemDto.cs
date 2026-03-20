using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Model.Entity;

namespace Inventory.Model.Dto.Create;

public class CreateLocationItemDto
{
    public required int LocationId { get; set; }
    public required int TargetQuantity { get; set; }
    public required int Quantity { get; set; }

    public ILocationItem UnpackDto()
    {
        return new LocationItem() {Quantity = Quantity, TargetQuantity = TargetQuantity, LocationId = LocationId};
    }
}
