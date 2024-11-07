using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UMBIT.Precatorios.API.Bootstrapper
{
    public static class AzureStorageConfigurate
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
        {
            var conection = configuration.GetConnectionString(nameof(BlobServiceClient));
            services.AddSingleton<BlobServiceClient>(new BlobServiceClient(conection));
            return services;
        }
    }
}
