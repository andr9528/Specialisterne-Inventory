using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableProduct : ISearchableProduct
    {
        /// <inheritdoc />
        public int Id { get; set; } = 0;

        /// <inheritdoc />
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc />
        public decimal Price { get; set; } = 0;

        /// <inheritdoc />
        public int CategoryId { get; set; } = 0;
    }
}
