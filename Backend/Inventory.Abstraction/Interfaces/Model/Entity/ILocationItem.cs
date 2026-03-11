using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Entity;

public interface ILocationItem: IEntity
{
    int Quantity { get; set; } 
    int TargetQuantity { get; set; }
}
