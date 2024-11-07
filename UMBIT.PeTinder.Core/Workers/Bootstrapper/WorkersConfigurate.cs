using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using UMBIT.Precatorios.SDK.Workers.Interfaces;

namespace UMBIT.Precatorios.SDK.Workers.Bootstrapper
{
    public static class WorkersConfigurate
    {
        public static IServiceCollection AddWorkers(this IServiceCollection services)
        {
            var wWorker = new WorkersWrapped();
            wWorker.AddWorkers(services);
            return services;
        }
    }
    
    class WorkersWrapped
    {
        public  IServiceCollection AddWorkers(IServiceCollection services)
        {
            var assemblys = AppDomain.CurrentDomain.GetAssemblies()
                                                    .Where(a => a.GetTypes().Any(t =>
                                                    t.IsAbstract == false &&
                                                    t.IsInterface == false &&
                                                    t.IsClass == true &&
                                                    t.IsAssignableTo(typeof(IWorker))));
            foreach (var assembly in assemblys)
            {
                foreach (var worker in assembly.GetTypes().Where(t => t.IsInterface == false && t.IsAssignableTo(typeof(IWorker))))
                {
                    Type type = typeof(WorkersWrapped);
                    MethodInfo methodInfo = type.GetMethod(nameof(AddHostedService));
                    MethodInfo genericMethod = methodInfo.MakeGenericMethod(worker);
                    genericMethod.Invoke(this, new object[] { services });
                }
            }

            return services;
        }
        public IServiceCollection AddHostedService<THostedService>(IServiceCollection services)
            where THostedService : class, IHostedService =>
                services.AddHostedService<THostedService>();
    }
}
