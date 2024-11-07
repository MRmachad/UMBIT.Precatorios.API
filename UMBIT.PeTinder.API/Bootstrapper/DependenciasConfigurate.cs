using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UMBIT.Precatorios.Dominio.Entidades.auth;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;
using UMBIT.Precatorios.Dominio.Servicos.auth;
using UMBIT.Precatorios.Dominio.Servicos.gerenciamento;

namespace UMBIT.Precatorios.API.Bootstrapper
{
    public static class DependenciasConfigurate
    {
        public static IServiceCollection AddDependencias(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<AuthenticationSettings>(configuration.GetSection(nameof(AuthenticationSettings)));
            services.AddScoped<IServicoDeToken, ServicoDeToken>()
                    .AddScoped<IServicoDeAutorizacao, ServicoDeAutorizacao>();

            services.AddScoped<IServicoDeUsuario<Usuario>, ServicoDeUsuario>();
            services.AddScoped<IServicoDeAutenticacao, ServicoDeAutenticacao<Usuario>>();

            services.AddScoped<IServicoDeGerenciamentoProcesso, ServicoDeGerenciamento>();

            return services;
        }
    }
}
