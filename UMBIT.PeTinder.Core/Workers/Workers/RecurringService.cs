using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;
using UMBIT.Precatorios.SDK.Workers.Basicos.Enum;
using UMBIT.Precatorios.SDK.Workers.Basicos.Utilitarios;

namespace UMBIT.Precatorios.SDK.Workers.Workers
{
    public abstract class RecurringService : BaseWorker
    {
        private Timer? Timer = null;

        private bool InicioImediato;
        private uint RecorrenciaMins;
        public RecurringService
            (uint RecorrenciaMins,
            IServiceScopeFactory serviceScopeFactory,
            bool inicioImediato = false) : base( serviceScopeFactory)
        {
            this.InicioImediato = inicioImediato;
            this.RecorrenciaMins = RecorrenciaMins;
        }
        public RecurringService(
            Recorrencia recorrencia,
            IServiceScopeFactory serviceScopeFactory,
            bool inicioImediato = false) : base( serviceScopeFactory)
        {
            this.InicioImediato = inicioImediato;
            this.RecorrenciaMins = UtilitarioDeRecorrencia.ObtenhaMinutos(recorrencia);
        }

        private void DoWork(object? state)
        {
            try
            {
                MainFunction((CancellationToken)state);
            }
            catch (Exception ex)
            {
            }
        }

        public override void Dispose()
        {
            Timer?.Dispose();
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            Timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken stoppingToken)
        {
            var hostLife = ObtenhaServico<IHostApplicationLifetime>();

            hostLife.ApplicationStarted.Register(async () =>
            {
                await Task.Delay(1000 * 10);

                if (InicioImediato) { DoWork(stoppingToken); }

                Timer = new Timer(DoWork, stoppingToken, TimeSpan.Zero, TimeSpan.FromMinutes(this.RecorrenciaMins));
            });

            return Task.CompletedTask;
        }

        public override abstract Task MainFunction(CancellationToken stoppingToken);
    }
}
