using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Searchable
{
    public class SearchableOrder : ISearchableOrder
    {
        /// <inheritdoc />
        public int Id { get; set; }

        /// <inheritdoc />
        public OrderStatus Status { get; set; }

        /// <inheritdoc />
        public Guid ReferenceId { get; set; }

        /// <inheritdoc />
        public int? LocationId { get; set; }
    }
}
