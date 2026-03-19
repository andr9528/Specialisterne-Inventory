using Inventory.Model.Entity;
using Inventory.Persistence.Core;
using Inventory.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configuration;

public class LocationItemConfiguration : EntityConfiguration<LocationItem>
{
    /// <inheritdoc />
    public LocationItemConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<LocationItem> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => (Location)x.Location).WithMany(x => (ICollection<LocationItem>)x.Products)
            .HasForeignKey(x => x.LocationId);
        builder.HasOne(x => (Product)x.Product).WithMany(x => (ICollection<LocationItem>)x.Locations)
            .HasForeignKey(x => x.ProductId);

        builder.Ignore(x => x.Status);

        builder.HasIndex(x => new {x.LocationId, x.ProductId}).IsUnique();
    }
}
