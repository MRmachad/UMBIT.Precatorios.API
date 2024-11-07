using Microsoft.Extensions.Hosting;

namespace UMBIT.Precatorios.SDK.Workers.Interfaces
{
    public interface IWorker : IHostedService
    {
        abstract Task MainFunction(CancellationToken stoppingToken);
    }
}
