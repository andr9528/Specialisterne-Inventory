using Inventory.Model.Entity;
using Inventory.Persistence.Core;
using Inventory.Persistence.Core.Abstraction;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Persistence.Configuration;

public class LocationConfiguration : EntityConfiguration<Location>
{
    /// <inheritdoc />
    public LocationConfiguration(DatabaseType type) : base(type)
    {
    }

    public override void Configure(EntityTypeBuilder<Location> builder)
    {
        base.Configure(builder);
    }
}
