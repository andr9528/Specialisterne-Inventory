using Inventory.Abstraction.Interfaces.Model.Searchable;
using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Entity;

public interface IOrder : ISearchableOrder, IEntity
{
    ICollection<IOrderItem> Products { get; set; }
    ILocation Location { get; set; }
}
