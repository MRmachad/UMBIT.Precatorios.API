using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;

namespace UMBIT.Precatorios.SDK.Notificacao.Bootstrapper
{
    public static class NotificacaoBootstrapper
    {
        public static IServiceCollection AdicionarNotificacao(this IServiceCollection services)
        {
            services.AddScoped<INotificador, Notificador>();

            return services;
        }
    }
}
