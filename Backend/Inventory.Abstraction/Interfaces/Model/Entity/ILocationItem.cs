using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Model.Searchable;
using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Entity;

public interface ILocationItem: ISearchableLocationItem, IEntity
{
    IProduct Product { get; set; }
    ILocation Location { get; set; }

    int TargetQuantity { get; set; }

    InventoryStatus Status { get; }
}
