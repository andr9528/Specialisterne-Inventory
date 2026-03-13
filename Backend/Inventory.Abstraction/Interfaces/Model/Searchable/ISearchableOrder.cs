using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Searchable;

public interface ISearchableOrder : ISearchable
{
    OrderStatus Status { get; set; }
    Guid ReferenceId { get; set; }
    int? LocationId { get; set; }
}
