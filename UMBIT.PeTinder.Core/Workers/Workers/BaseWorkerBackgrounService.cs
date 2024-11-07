using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;
using UMBIT.Precatorios.SDK.Workers.Interfaces;

namespace UMBIT.Precatorios.SDK.Workers.Workers
{
    public abstract class BaseWorkerBackgrounService : BackgroundService, IWorker
    {
        protected readonly IServiceScopeFactory ServiceScopeFactory;

        public BaseWorkerBackgrounService( IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }
        protected T ObtenhaServico<T>()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            return scope.ServiceProvider.GetService<T>();

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var hostLife = ObtenhaServico<IHostApplicationLifetime>();

                hostLife.ApplicationStarted.Register(async () =>
                {
                    await Task.Delay(10000);    
                     MainFunction(stoppingToken);
                });

                return Task.CompletedTask;

            }
            catch (Exception ex)
            {
                return Task.CompletedTask;
            }
        }

        public abstract Task MainFunction(CancellationToken stoppingToken);

    }
}
