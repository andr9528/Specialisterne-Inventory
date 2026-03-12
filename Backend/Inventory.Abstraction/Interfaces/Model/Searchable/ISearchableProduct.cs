using Inventory.Abstraction.Interfaces.Persistence;

namespace Inventory.Abstraction.Interfaces.Model.Searchable;

public interface ISearchableProduct : ISearchable
{
    string Name { get; set; }

    decimal Price { get; set; }

    int CategoryId { get; set; }
}
