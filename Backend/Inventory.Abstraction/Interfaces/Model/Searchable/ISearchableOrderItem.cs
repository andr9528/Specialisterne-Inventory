using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Searchable;

public interface ISearchableOrderItem : ISearchable
{
    int OrderId { get; set; }
    int ProductId { get; set; }
}
