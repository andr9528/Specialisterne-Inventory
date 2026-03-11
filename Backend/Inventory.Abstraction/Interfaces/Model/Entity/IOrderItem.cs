using Inventory.Abstraction.Interfaces.Model.Searchable;
using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Entity;

public interface IOrderItem: ISearchableOrderItem, IEntity
{
    int Quantity { get; set; } 
}
