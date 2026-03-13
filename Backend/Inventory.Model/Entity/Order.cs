using System.Text.Json.Serialization;
using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Model.Entity;
using Inventory.Abstraction.Interfaces.Model.Searchable;

namespace Inventory.Model.Entity
{
    public class Order : IOrder
    {
        private int id;

        /// <inheritdoc />
        public int Id
        {
            get => id;
            set => throw new InvalidOperationException(
                $"{nameof(Id)} cannot be changed after creation of {nameof(Order)} entity");
        }

        /// <inheritdoc />
        public byte[] Version { get; set; }

        /// <inheritdoc />
        public DateTime CreatedDateTime { get; set; }

        /// <inheritdoc />
        public DateTime UpdatedDateTime { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public ICollection<IOrderItem> Products { get ; set ; }

        /// <inheritdoc />
        public ILocation Location { get; set; }

        /// <inheritdoc />
        public OrderStatus Status { get; set; }

        /// <inheritdoc />
        public Guid ReferenceId { get; set; }

        /// <inheritdoc />
        public int? LocationId { get; set; }

        /// <summary>
        /// Constructor for Entity Framework Core to use.
        /// Enables the 'Id' to be immutable after the entity is created, which is a good practice for entities.
        /// Use of [JsonConstructor] is what makes Entity Framework Core use this constructor instead of the parameterless one, which is the default behavior.
        /// </summary>
        /// <param name="id"></param>
        [JsonConstructor]
        private Order(int id, List<OrderItem> products)
        {
            this.id = id;
            this.Products = products.Cast<IOrderItem>().ToList();
        }

        public Order()
        {
        }
    }
}
