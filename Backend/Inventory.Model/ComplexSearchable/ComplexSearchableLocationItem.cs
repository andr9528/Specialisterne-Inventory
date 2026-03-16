using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Searchable;

namespace Inventory.Model.ComplexSearchable;

public class ComplexSearchableLocationItem : IComplexSearchable<SearchableLocationItem>
{
    /// <inheritdoc />
    public SearchableLocationItem Searchable { get; set; }

    public int? MinimumItemsInStock { get; set; }

    public int? MinimumItemsReserved { get; set; }
}
