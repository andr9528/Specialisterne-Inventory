using Inventory.Model.Entity;
using Inventory.Persistence.Core;
using Inventory.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configuration;

public class OrderItemConfiguration : EntityConfiguration<OrderItem>
{
    /// <inheritdoc />
    public OrderItemConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => (Order)x.Order).WithMany(x => (ICollection<OrderItem>)x.Products)
            .HasForeignKey(x => x.OrderId);
        builder.HasOne(x => (Product)x.Product).WithMany(x => (ICollection<OrderItem>)x.Orders)
            .HasForeignKey(x => x.ProductId);
    }
}
