using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableOrder : ISearchableOrder
    {
        /// <inheritdoc />
        public int Id { get; set; }
    }
}
