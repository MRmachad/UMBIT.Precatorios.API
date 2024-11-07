using Microsoft.Extensions.DependencyInjection;
using UMBIT.Precatorios.SDK.API.Models.Fabrica;

namespace UMBIT.Precatorios.SDK.API.Bootstrapper
{
    public static class FabricaConfigurate
    {
        public static IServiceCollection AddFabricaGenerica(this IServiceCollection services)
        {
            FabricaGenerica.services = services.BuildServiceProvider();
            return services;
        }
    }
}
