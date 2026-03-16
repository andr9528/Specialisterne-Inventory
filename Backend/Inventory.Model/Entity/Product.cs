using System.Text.Json.Serialization;
using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Model.Entity;

namespace Inventory.Model.Entity
{
    public class Product : IProduct
    {
        private int id;

        /// <inheritdoc />
        public int Id
        {
            get => id;
            set => throw new InvalidOperationException(
                $"{nameof(Id)} cannot be changed after creation of {nameof(Product)} entity");
        }

        /// <inheritdoc />
        public uint Version { get; set; }

        /// <inheritdoc />
        public DateTime CreatedDateTime { get; set; }

        /// <inheritdoc />
        public DateTime UpdatedDateTime { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public ICollection<ILocationItem> Locations { get; set; } = new List<ILocationItem>();

        /// <inheritdoc />
        public ICategory Category { get; set; }

        /// <inheritdoc />
        public decimal Price { get; set; }

        /// <inheritdoc />
        public ICollection<IOrderItem> Orders { get; set; } = new List<IOrderItem>();

        /// <inheritdoc />
        public InventoryStatus Status
        {
            get => GetStatus();
        }

        private InventoryStatus GetStatus()
        {
            return LocationItem.GetStatus(TotalQuantity, TotalTargetQuantity);
        }

        /// <inheritdoc />
        public int TotalQuantity
        {
            get => GetTotalQuantity();
        }

        private int GetTotalQuantity()
        {
            return Locations.Sum(x => x.Quantity);
        }

        /// <inheritdoc />
        public int TotalTargetQuantity
        {
            get => GetTotalTargetQuantity();
        }

        private int GetTotalTargetQuantity()
        {
            return Locations.Sum(x => x.TargetQuantity);
        }

        /// <inheritdoc />
        public int CategoryId { get; set; }

        /// <summary>
        /// Constructor for Entity Framework Core to use.
        /// Enables the 'Id' to be immutable after the entity is created, which is a good practice for entities.
        /// Use of [JsonConstructor] is what makes Entity Framework Core use this constructor instead of the parameterless one, which is the default behavior.
        /// </summary>
        /// <param name="id"></param>
        [JsonConstructor]
        private Product(int id, List<LocationItem> locations, List<OrderItem> orders, Category category)
        {
            this.id = id;
            Locations = locations.Cast<ILocationItem>().ToList();
            Orders = orders.Cast<IOrderItem>().ToList();
            Category = (ICategory)category;
        }

        public Product()
        {
        }
    }
}
