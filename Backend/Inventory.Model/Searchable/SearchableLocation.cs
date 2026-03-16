using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableLocation : ISearchableLocation
    {
        /// <inheritdoc />
        public int Id { get; set; } = 0;

        /// <inheritdoc />
        public string Name { get; set; } = string.Empty;
    }
}
