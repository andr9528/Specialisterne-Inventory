using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Abstraction.Interfaces.Startup
{
    public interface IServiceStartupModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}