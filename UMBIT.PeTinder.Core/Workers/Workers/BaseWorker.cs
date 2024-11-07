using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;
using UMBIT.Precatorios.SDK.Workers.Interfaces;

namespace UMBIT.Precatorios.SDK.Workers.Workers
{
    /// <summary>
    /// Todos o parametros de contrutores injetados das classe derivadas devem ser singleton por que workers no scopo .Net são tambem,
    /// caso contrario devem ser obtido atraves do 'serviceScopeFactory' no metodo 'ObtenhaServico'.
    /// </summary>
    public abstract class BaseWorker : IWorker, IDisposable
    {
        protected readonly IServiceScopeFactory ServiceScopeFactory;

        public BaseWorker( IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }
        protected T ObtenhaServico<T>()
        {
            using var scope = ServiceScopeFactory.CreateScope();
            return scope.ServiceProvider.GetService<T>();
        }

        public abstract Task MainFunction(CancellationToken stoppingToken);

        public abstract Task StartAsync(CancellationToken cancellationToken);

        public abstract Task StopAsync(CancellationToken cancellationToken);

        public abstract void Dispose();

    }
}
