using Inventory.Model.Entity;
using Inventory.Persistence.Core;
using Inventory.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configuration;

public class CategoryConfiguration: EntityConfiguration<Category>
{
    /// <inheritdoc />
    public CategoryConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);
    }
}
