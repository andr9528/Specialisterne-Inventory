using Inventory.Model.Entity;
using Inventory.Persistence.Core;
using Inventory.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configuration;

public class OrderConfiguration : EntityConfiguration<Order>
{
    /// <inheritdoc />
    public OrderConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.HasIndex(x => new {x.ReferenceId, x.LocationId}).IsUnique();

        builder.HasOne(x => (Location) x.Location).WithMany(x => (ICollection<Order>) x.Orders)
            .HasForeignKey(x => x.LocationId);
        builder.Property(x => x.LocationId).IsRequired(false);
    }
}
