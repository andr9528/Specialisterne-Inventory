using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableCategory : ISearchableCategory
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

    }
}
