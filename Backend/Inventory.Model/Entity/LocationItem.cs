using System.Diagnostics;
using System.Text.Json.Serialization;
using Inventory.Abstraction.Enum;
using Inventory.Abstraction.Interfaces.Model.Entity;

namespace Inventory.Model.Entity;

public class LocationItem : ILocationItem
{
    private int id;

    /// <inheritdoc />
    public int Id
    {
        get => id;
        set => throw new InvalidOperationException(
            $"{nameof(Id)} cannot be changed after creation of {nameof(LocationItem)} entity");
    }

    /// <inheritdoc />
    public uint Version { get; set; }

    /// <inheritdoc />
    public DateTime CreatedDateTime { get; set; }

    /// <inheritdoc />
    public DateTime UpdatedDateTime { get; set; }

    /// <inheritdoc />
    public int Quantity { get ; set ; }

    /// <inheritdoc />
    public int ReservedQuantity { get; set; }

    /// <inheritdoc />
    public int TargetQuantity { get ; set ; }

    /// <inheritdoc />
    public InventoryStatus Status
    {
        get => GetStatus(Quantity, TargetQuantity);
    }

    /// <inheritdoc />
    public IProduct Product { get; set; }

    /// <inheritdoc />
    public ILocation Location { get; set; }

    /// <inheritdoc />
    public int LocationId { get; set; }

    /// <inheritdoc />
    public int ProductId { get; set; }

    /// <summary>
    /// Constructor for Entity Framework Core to use.
    /// Enables the 'Id' to be immutable after the entity is created, which is a good practice for entities.
    /// Use of [JsonConstructor] is what makes Entity Framework Core use this constructor instead of the parameterless one, which is the default behavior.
    /// </summary>
    /// <param name="id"></param>
    [JsonConstructor]
    private LocationItem(int id, Product product, Location location)
    {
        this.id = id;
        Product = (IProduct)product;
        Location = (ILocation)location;
    }
    
    public LocationItem()
    {
    }

    public static InventoryStatus GetStatus(int quantity, int target)
    {
        if (quantity == 0)
            return InventoryStatus.OUT_OF_STOCK;

        if (quantity < target)
            return InventoryStatus.LOW_STOCK;

        if (quantity >= target)
            return InventoryStatus.IN_STOCK;

        return InventoryStatus.UNKNOWN;
    }
}
