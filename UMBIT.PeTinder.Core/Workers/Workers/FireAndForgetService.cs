using Microsoft.Extensions.DependencyInjection;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;

namespace UMBIT.Precatorios.SDK.Workers.Workers
{
    public abstract class FireAndForgetService : BaseWorkerBackgrounService
    {
        protected FireAndForgetService(IServiceScopeFactory serviceScopeFactory) : base( serviceScopeFactory)
        {
        }

        public override abstract Task MainFunction(CancellationToken stoppingToken);
    }
}
