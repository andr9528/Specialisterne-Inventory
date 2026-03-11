using Inventory.Abstraction.Interfaces.Model.Searchable;
using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Entity;

public interface IProduct : ISearchableProduct, IEntity
{
    ICategory Category { get; set; }
    ICollection<ILocationItem> Locations { get; set; }
    ICollection<IOrderItem> Orders { get; set; }
}
