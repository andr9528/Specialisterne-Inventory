using Inventory.Abstraction.Interfaces.Model.Searchable;
using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Entity;

public interface IProduct : ISearchableProduct, IEntity
{

    ICollection<ILocationItem> Locations { get; set; }
}
