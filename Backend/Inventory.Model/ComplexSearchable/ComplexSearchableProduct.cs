using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Searchable;

namespace Inventory.Model.ComplexSearchable;

public class ComplexSearchableProduct : IComplexSearchable<SearchableProduct>
{
    /// <inheritdoc />
    public SearchableProduct Searchable { get; set; }

    public bool? IncludeLocations { get; set; }
    public bool? IncludeOrders { get; set; }
}
