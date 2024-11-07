using Microsoft.Extensions.DependencyInjection;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;

namespace UMBIT.Precatorios.SDK.Workers.Workers
{
    public abstract class DelayedService : BaseWorkerBackgrounService
    {
        private TimeSpan Delay;
        public DelayedService(
            TimeSpan timeSpan,
            IServiceScopeFactory serviceScopeFactory) : base( serviceScopeFactory)
        {
            Delay = timeSpan;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Delay(Delay).Wait();
            return base.StartAsync(cancellationToken);
        }
        public override abstract Task MainFunction(CancellationToken stoppingToken);
    }
}
