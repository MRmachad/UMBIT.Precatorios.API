using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using UMBIT.Precatorios.SDK.SignalR.Interfaces;
using UMBIT.Precatorios.SDK.SignalR.Modelos;

namespace UMBIT.Precatorios.SDK.SignalR.Cliente
{
    public class SignalRClient : ISignalRClient
    {
        private HubConnection Conexao;
        private ILogger<SignalRClient> Logger;
        public SignalRClient(IOptions<SignalClientSettings> options, ILogger<SignalRClient> logger, IServer server, IHostApplicationLifetime hostApplicationLifetime)
        {
            Logger = logger;

            hostApplicationLifetime.ApplicationStarted.Register(() =>
            {
                var url = $"{options.Value?.SignalURL ?? $"{server.Features?.Get<IServerAddressesFeature>()?.Addresses.FirstOrDefault()}"}/{options.Value?.Hub ?? "hub"}";

                Logger.Log(LogLevel.Information, $"Conectando em servidor hub signalr '{url}'.");

                Conexao = new HubConnectionBuilder()
                    .WithUrl(new Uri(url))
                    .WithAutomaticReconnect()
                    .Build();

                Conexao.Closed += async (error) =>
                {
                    this.Logger.LogError(error, "Erro na Conexão!");

                    await Task.Delay(new Random().Next(0, 5) * 1000);
                    await Conexao.StartAsync();
                };

                Conexao.StartAsync().Wait();

            });
        }


        public async Task EmitaAtualizacao(string metodo, object dados, string grupo = null)
        {
            VerifiqueConnectClient();

            try
            {
                this.Logger.LogInformation($"Emitindo atualização via SignalR. {grupo}, {metodo}");
                await this.Conexao?.InvokeAsync("Atualizar", grupo ?? metodo, metodo, dados);
            }
            catch (Exception ex)
            {
                this.Conexao?.StopAsync();

                this.Logger.LogError(ex, "Erro na emissão de atualização!");
            }
        }
        public async Task RecebaAtualizacao<T>(string metodo, Action<T> handler, string grupo = null)
        {
            VerifiqueConnectClient();

            try
            {
                this.Logger.LogInformation("Configurando handler de atualização via SignalR");
                await this.Conexao?.InvokeAsync("Registrar", grupo ?? metodo);
                this.Conexao?.On<T>(metodo, handler);
            }
            catch (Exception ex)
            {
                this.Conexao?.StopAsync();

                this.Logger.LogError(ex, "Erro na recepção de atualização!");
            }
        }

        private void VerifiqueConnectClient()
        {
            if (this.Conexao?.State != HubConnectionState.Connected)
            {
                this.Logger.LogInformation("Cliente Desconectado!");

                Conexao?.StartAsync().Wait();
            }
        }



    }
}
