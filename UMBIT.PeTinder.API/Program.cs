using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UMBIT.Precatorios.API.Bootstrapper;
using UMBIT.Precatorios.Dominio.Configuradores;
using UMBIT.Precatorios.Infraestrutura.Contextos;
using UMBIT.Precatorios.SDK.Notificacao.Bootstrapper;
using UMBIT.Precatorios.SDK.Repositorio.Bootstrapper;
using UMBIT.Precatorios.SDK.SignalR.Bootstrapper;
using UMBIT.Precatorios.SDK.Workers.Bootstrapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataBase<AppContexto>(builder.Configuration, "UMBIT.Precatorios.API");
builder.Services.AddIdentityConfiguration(builder.Configuration, "UMBIT.Precatorios.API");
builder.Services.AddJudicialConfiguration(builder.Configuration, "UMBIT.Precatorios.API");

builder.Services
    .AddSignalRClient(builder.Configuration)
    .AdicionarNotificacao()
    .AddSignalRHub()
    .AddStorage(builder.Configuration)
    .AddWorkers();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IdentitySettings).Assembly));

builder.Services.AddApp()
                .AddDependencias(builder.Configuration);

var app = builder.Build();

app.UseApp();
app.UseMigrations();
app.UseIdentityConfiguration();
app.UseJudicialConfiguration();
app.UseSignalRHub();
app.Run();