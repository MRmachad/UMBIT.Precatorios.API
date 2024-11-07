using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using UMBIT.Precatorios.Core.API.Basicos.Atributos;
using UMBIT.Precatorios.SDK.API.Extensoes;

namespace UMBIT.Precatorios.API.Bootstrapper
{
    public static class AppConfigurate
    {
        public static IServiceCollection AddApp(this IServiceCollection services)
        {
            services.AddCors();
            services.AddControllers(o =>
            {
                o.Filters.Add(new ODataQueryAttribute());
            });
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services
            .AddMvc()
            .AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.WriteIndented = true;
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            })
            .AddOData((o) =>
            {
                o.EnableQueryFeatures(1000);
            });

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.ToString());
            });

            return services;
        }

        public static IApplicationBuilder UseApp(this IApplicationBuilder app)
        {
            app.UseCors(t =>
            {
                t.AllowAnyOrigin();
                t.AllowAnyMethod();
                t.AllowAnyHeader();
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
