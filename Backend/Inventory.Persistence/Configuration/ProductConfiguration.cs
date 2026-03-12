using Inventory.Model.Entity;
using Inventory.Persistence.Core;
using Inventory.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configuration;

public class ProductConfiguration: EntityConfiguration<Product>
{
    /// <inheritdoc />
    public ProductConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => (Category)x.Category).WithMany(x => (ICollection<LocationItem>)x.Products)
    .HasForeignKey(x => x.CategoryId);
    }
}
