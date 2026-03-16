using System.Text.Json.Serialization;
using Inventory.Abstraction.Interfaces.Model.Entity;

namespace Inventory.Model.Entity;

public class OrderItem : IOrderItem
{
    private int id;

    /// <inheritdoc />
    public int Id
    {
        get => id;
        set => throw new InvalidOperationException(
            $"{nameof(Id)} cannot be changed after creation of {nameof(OrderItem)} entity");
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
    public IProduct Product { get; set; }

    /// <inheritdoc />
    public IOrder Order { get; set; }

    /// <inheritdoc />
    public int ProductId { get; set; }

    /// <inheritdoc />
    public int OrderId { get; set; }


    /// <summary>
    /// Constructor for Entity Framework Core to use.
    /// Enables the 'Id' to be immutable after the entity is created, which is a good practice for entities.
    /// Use of [JsonConstructor] is what makes Entity Framework Core use this constructor instead of the parameterless one, which is the default behavior.
    /// </summary>
    /// <param name="id"></param>
    [JsonConstructor]
    private OrderItem(int id, Product product, Order order)
    {
        this.id = id;
        Product = (IProduct)product;
        Order = (IOrder)order;
    }
    
    public OrderItem()
    {
    }
}
