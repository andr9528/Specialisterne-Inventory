using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Entity;

public interface IOrderItem: IEntity
{
    int Quantity { get; set; } 
}
