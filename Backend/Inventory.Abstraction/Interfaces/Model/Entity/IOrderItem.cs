using Inventory.Abstraction.Interfaces.Model.Searchable;
using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Entity;

public interface IOrderItem: ISearchableOrderItem, IEntity
{
    IOrder Order { get; set; }
    IProduct Product { get; set; }  
    int Quantity { get; set; } 
}
