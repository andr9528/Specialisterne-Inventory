using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableOrderItem : ISearchableOrderItem
    {
        /// <inheritdoc />
        public int Id { get; set; } = 0;
        /// <inheritdoc />
        public int OrderId { get; set; } = 0;
        /// <inheritdoc />
        public int ProductId { get; set; } = 0;

    }
}
