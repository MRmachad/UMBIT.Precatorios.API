using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace UMBIT.Precatorios.SDK.Repositorio.Bootstrapper
{
    public static class MigrationConfigurate
    {
        public static IApplicationBuilder UseMigrations(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope?.ServiceProvider.GetRequiredService<DbContext>();

                context?.Database.EnsureCreated();

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

                        throw new Exception("Falha ao executar Migration");
                    }

                }
            }

            return app;
        }
    }
}
