using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using UMBIT.Precatorios.API.Extensao;
using UMBIT.Precatorios.Dominio.Configuradores;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;
using UMBIT.Precatorios.Dominio.Servicos.auth;
using UMBIT.Precatorios.Infraestrutura.Contexto;

namespace UMBIT.Precatorios.API.Bootstrapper
{
    public static class IdentityConfigurate
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration, string nomeApi)
        {
            var conexao = configuration.GetConnectionString("identity");
            services.AddDbContext<IdentityContext>(options => options.UseMySql(conexao, ServerVersion.AutoDetect(conexao), b => b.MigrationsAssembly(nomeApi)));

            services.AddIdentity<Usuario, Role>((options) =>
                    {
                    })
                    .AddErrorDescriber<IdentityMensagensPortugues>()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddDefaultTokenProviders();

            var sectionSettings = configuration.GetSection(nameof(IdentitySettings));

            services.Configure<IdentitySettings>(sectionSettings);
            var identitySettings = sectionSettings.Get<IdentitySettings>() ?? new IdentitySettings();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = identitySettings.RequiredLength;
                options.Password.RequireDigit = identitySettings.RequireDigit;
                options.User.RequireUniqueEmail = identitySettings.RequireUniqueEmail;
                options.Password.RequireLowercase = identitySettings.RequireLowercase;
                options.Password.RequireUppercase = identitySettings.RequireUppercase;
                options.Password.RequiredUniqueChars = identitySettings.RequiredUniqueChars;
                options.Lockout.AllowedForNewUsers = identitySettings.AllowedForNewUsers;
                options.Lockout.MaxFailedAccessAttempts = identitySettings.MaxFailedAccessAttempts;
                options.SignIn.RequireConfirmedEmail = identitySettings.RequireConfirmedEmail;
                options.Password.RequireNonAlphanumeric = identitySettings.RequireNonAlphanumeric;
                options.User.AllowedUserNameCharacters = identitySettings.AllowedUserNameCharacters;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.DefaultLockoutTimeSpan);
            });

            services.AddScoped<IServicoDeCadastro, ServicoDeCadastro>();

            return services;
        }

        public static IApplicationBuilder UseIdentityConfiguration(this IApplicationBuilder app)
        {

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                var context = serviceScope?.ServiceProvider.GetRequiredService<IdentityContext>();

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
