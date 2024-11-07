using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using UMBIT.Precatorios.Infraestrutura.Contextos;
using UMBIT.Precatorios.Infraestrutura.Repositorios;

namespace UMBIT.Precatorios.API.Bootstrapper
{
    public static class JudicialConfigurate
    {
        public static IServiceCollection AddJudicialConfiguration(this IServiceCollection services, IConfiguration configuration, string nomeApi)
        {
            var conexao = configuration.GetConnectionString("judicial");
            services.AddDbContext<JudicialContexto>(options => options.UseMySql(conexao, ServerVersion.AutoDetect(conexao), b => b.MigrationsAssembly(nomeApi)));
            services.AddScoped<IRepositorioDataProcesso, RepositorioDataProcesso>();
            return services;
        }

        public static IApplicationBuilder UseJudicialConfiguration(this IApplicationBuilder app)
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope?.ServiceProvider.GetRequiredService<JudicialContexto>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (Exception)
                    {
                        if (!context?.Database.EnsureCreated() ?? false)
                            context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }

                }
            }

            return app;
        }
    }
}
