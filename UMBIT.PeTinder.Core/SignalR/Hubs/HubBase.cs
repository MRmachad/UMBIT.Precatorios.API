using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;

namespace UMBIT.Precatorios.SDK.SignalR.Hubs
{
    public abstract class HubBase: Hub
    {
        public async Task Registrar(string grupo)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, grupo);
        }

        public async Task Sair(string grupo)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, grupo);
        }

        public async Task Atualizar(string grupo, string metodo, object dados)
        {
           await Clients.Group(grupo).SendAsync(metodo, dados);
        }
    }
}
