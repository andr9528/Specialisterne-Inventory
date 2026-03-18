using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableOrder : ISearchableOrder
    {
        /// <inheritdoc />
        public int Id { get; set; } = 0;

        /// <inheritdoc />
        public OrderStatus Status { get; set; } = OrderStatus.UNKNOWN;

        /// <inheritdoc />
        public Guid ReferenceId { get; set; } = Guid.Empty;

        /// <inheritdoc />
        public int? LocationId { get; set; } = 0;
    }
}
