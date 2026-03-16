using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Searchable;

public interface ISearchableLocationItem : ISearchable
{
    int LocationId { get; set; }
    int ProductId { get; set; }
    int Quantity { get; set; }
    int ReservedQuantity { get; set; }
}
