using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableLocationItem : ISearchableLocationItem
    {
        /// <inheritdoc />
        public int LocationId { get; set; }
        /// <inheritdoc />
        public int ProductId { get; set; }
        /// <inheritdoc />
        public int Quantity { get; set; }

        /// <inheritdoc />
        public int ReservedQuantity { get; set; }

        /// <inheritdoc />
        public int Id { get; set; }
    }
}
