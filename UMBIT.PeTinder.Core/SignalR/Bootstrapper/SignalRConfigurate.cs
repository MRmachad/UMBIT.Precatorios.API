using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UMBIT.Precatorios.SDK.SignalR.Basicos.Atributos;
using UMBIT.Precatorios.SDK.SignalR.Cliente;
using UMBIT.Precatorios.SDK.SignalR.Hubs;
using UMBIT.Precatorios.SDK.SignalR.Interfaces;
using UMBIT.Precatorios.SDK.SignalR.Modelos;

namespace UMBIT.Precatorios.SDK.SignalR.Bootstrapper
{
    public static class SignalRConfigurate
    {
        public static IServiceCollection AddSignalRHub(this IServiceCollection services)
        {
            services.AddSignalR();
            return services;
        }

        public static IServiceCollection AddSignalRClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SignalClientSettings>(configuration.GetSection(nameof(SignalClientSettings)));
            services.AddSingleton<ISignalRClient, SignalRClient>();
            return services;
        }

        public static IEndpointRouteBuilder UseSignalRHub(this IEndpointRouteBuilder routing)
        {
            var hubs = new SignalRsWrapped();

            return hubs.AddHubs(routing);
             
        }
        class SignalRsWrapped
        {
            public IEndpointRouteBuilder AddHubs(IEndpointRouteBuilder routing)
            {
                var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetTypes().Any(t => t.IsInterface == false && t.GetCustomAttribute<HubPathAttribute>() != null && t.IsClass == true && t.IsAssignableTo(typeof(HubBase))));
                foreach (var assembly in assemblys)
                {
                    foreach (var hub in assembly.GetTypes().Where(t => t.IsInterface == false && t.GetCustomAttribute<HubPathAttribute>() != null && t.IsAssignableTo(typeof(HubBase))))
                    {
                        var pathAtt = hub.GetCustomAttribute<HubPathAttribute>();
                        Type type = typeof(SignalRsWrapped);
                        MethodInfo methodInfo = type.GetMethod(nameof(AddHub));
                        MethodInfo genericMethod = methodInfo.MakeGenericMethod(hub);
                        genericMethod.Invoke(this, new object[] { routing, pathAtt?.Path });
                    }
                }

                return routing;
            }
            public void AddHub<hub>(IEndpointRouteBuilder routing, string path)
                where hub : HubBase =>
                routing.MapHub<hub>(path);
        }
    }
}
