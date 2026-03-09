using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableExample : ISearchableExample
    {
        /// <inheritdoc />
        public int Id { get; set; }
    }
}