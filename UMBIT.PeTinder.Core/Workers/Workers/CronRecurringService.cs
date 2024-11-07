using Cronos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;
using UMBIT.Precatorios.SDK.Workers.Basicos.Enum;
using UMBIT.Precatorios.SDK.Workers.Basicos.Utilitarios;

namespace UMBIT.Precatorios.SDK.Workers.Workers
{
    public abstract class CronRecurringService : BaseWorker
    {
        private bool InicioImediato;
        private System.Timers.Timer Timer;
        private readonly CronExpression Expression;

        protected CronRecurringService(
            string cronExpression,
            IServiceScopeFactory serviceScopeFactory,
            bool inicioImediato = false) : base(serviceScopeFactory)
        {
            InicioImediato = inicioImediato;
            Expression = CronExpression.Parse(cronExpression);
        }

        protected CronRecurringService(
            Recorrencia recorrencia,
            IServiceScopeFactory serviceScopeFactory,
            bool inicioImediato = false) : base(serviceScopeFactory)
        {
            InicioImediato = inicioImediato;
            Expression = CronExpression.Parse(UtilitarioDeRecorrencia.ObtenhaCron(recorrencia));
        }

        public override void Dispose()
        {
            Timer?.Dispose();
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Timer?.Stop();
            await Task.CompletedTask;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);

        }

        private async Task ScheduleJobRecurring(CancellationToken cancellationToken)
        {
            try
            {

                var next = Expression.GetNextOccurrence(DateTimeOffset.Now, TimeZoneInfo.Local);
                if (next.HasValue || InicioImediato)
                {
                    var delay = next.Value - DateTimeOffset.Now;

                    if (delay.TotalMilliseconds <= 0 || InicioImediato)
                    {
                        InicioImediato = false;

                        try
                        {
                            await MainFunction(cancellationToken);
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    Timer = new System.Timers.Timer(delay.TotalMilliseconds);
                    Timer.Elapsed += async (sender, args) =>
                    {
                        Timer.Dispose();
                        Timer = null;
                        if (!cancellationToken.IsCancellationRequested)
                        {

                            try
                            {
                                await MainFunction(cancellationToken);
                            }
                            catch (Exception ex)
                            {
                            }

                            await ScheduleJobRecurring(cancellationToken);
                        }
                    };
                    Timer.Start();
                }
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.CompletedTask;
            }
        }
        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            try
            {
                var hostLife = ObtenhaServico<IHostApplicationLifetime>();

                hostLife.ApplicationStarted.Register(async () =>
                {
                    await Task.Delay(1000 * 10);
                    await ScheduleJobRecurring(cancellationToken);
                });

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.CompletedTask;
            }
        }
        public override abstract Task MainFunction(CancellationToken cancellationToken);


    }
}
