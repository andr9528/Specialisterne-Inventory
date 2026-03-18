using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Searchable;

namespace Inventory.Model.ComplexSearchable;

public class ComplexSearchableProduct : IComplexSearchable<SearchableProduct>
{
    /// <inheritdoc />
    public SearchableProduct Searchable { get; set; } = new SearchableProduct();

    public bool? IncludeLocations { get; set; }
    public bool? IncludeOrders { get; set; }
    public InventoryStatus? HasInventoryStatus { get; set; }
    public string? CategoryNameContains { get; set; }
    public string? LocationNameContains { get; set; }
}
