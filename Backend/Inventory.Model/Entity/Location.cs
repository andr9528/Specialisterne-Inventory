using System.Text.Json.Serialization;
using Inventory.Abstraction.Interfaces.Model.Entity;

namespace Inventory.Model.Entity
{
    public class Location : ILocation
    {
        private int id;

        /// <inheritdoc />
        public int Id
        {
            get => id;
            set => throw new InvalidOperationException(
                $"{nameof(Id)} cannot be changed after creation of {nameof(Location)} entity");
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
        public ICollection<ILocationItem> Products { get ; set ; }

        /// <summary>
        /// Constructor for Entity Framework Core to use.
        /// Enables the 'Id' to be immutable after the entity is created, which is a good practice for entities.
        /// Use of [JsonConstructor] is what makes Entity Framework Core use this constructor instead of the parameterless one, which is the default behavior.
        /// </summary>
        /// <param name="id"></param>
        [JsonConstructor]
        private Location(int id, ICollection<ILocationItem> products)
        {
            this.id = id;
            Products = products;
        }

        public Location()
        {
        }
    }
}
