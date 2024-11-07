using System.Text.RegularExpressions;

namespace UMBIT.Precatorios.SDK.SignalR.Interfaces
{
    public interface ISignalRClient
    {
        Task EmitaAtualizacao(string metodo, object dados, string grupo = null);
        Task RecebaAtualizacao<T>(string metodo, Action<T> handler, string grupo = null);
    }
}
