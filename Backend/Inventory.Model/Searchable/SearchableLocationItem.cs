using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableLocationItem : ISearchableLocationItem
    {
        /// <inheritdoc />
        public int LocationId { get; set; } = 0;

        /// <inheritdoc />
        public int ProductId { get; set; } = 0;

        /// <inheritdoc />
        public int Quantity { get; set; } = 0;

        /// <inheritdoc />
        public int ReservedQuantity { get; set; } = 0;

        /// <inheritdoc />
        public int Id { get; set; } = 0;
    }
}
