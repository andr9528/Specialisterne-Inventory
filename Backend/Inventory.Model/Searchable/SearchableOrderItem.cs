using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableOrderItem : ISearchableOrderItem
    {
        /// <inheritdoc />
        public int Id { get; set; }
        /// <inheritdoc />
        public int OrderId { get; set; }
        /// <inheritdoc />
        public int ProductId { get; set; }

    }
}
