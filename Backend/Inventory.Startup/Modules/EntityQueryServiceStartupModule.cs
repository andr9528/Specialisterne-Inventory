using Microsoft.Extensions.DependencyInjection;
using Inventory.Abstraction.Interfaces.Persistence;
using Inventory.Abstraction.Interfaces.Startup;

namespace Inventory.Startup.Modules
{
    public class EntityQueryServiceStartupModule<TQuery, TEntity, TSearchable> : IServiceStartupModule
        where TQuery : class, IEntityQueryService<TEntity, TSearchable>
        where TEntity : class, IEntity
        where TSearchable : class, ISearchable, new()
    {
        /// <inheritdoc />
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEntityQueryService<TEntity, TSearchable>, TQuery>();
        }
    }
}
