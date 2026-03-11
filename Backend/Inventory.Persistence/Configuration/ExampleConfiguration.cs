using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Inventory.Model.Entity;
using Inventory.Persistence.Core;
using Inventory.Persistence.Core.Abstraction;

namespace Inventory.Persistence.Configuration
{
    public class ExampleConfiguration : EntityConfiguration<Example>
    {
        /// <inheritdoc />
        public ExampleConfiguration(DatabaseType type) : base(type)
        {
        }

        public override void Configure(EntityTypeBuilder<Example> builder)
        {
            base.Configure(builder);

            // Additional configuration for the 'Example' entity can be added here.
            // For instance, if there are any relationships, indexes, or constraints specific to 'Example', they can be defined here.
        }
    }
}