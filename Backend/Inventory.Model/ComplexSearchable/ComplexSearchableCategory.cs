using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Model.Searchable;

namespace Inventory.Model.ComplexSearchable;

public class ComplexSearchableCategory : IComplexSearchable<SearchableCategory>
{
    /// <inheritdoc />
    public SearchableCategory Searchable { get; set; } = new SearchableCategory();
    public string? CategoryNameContains { get; set; }
}
