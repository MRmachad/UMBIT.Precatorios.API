using System.Runtime.CompilerServices;
using  UMBIT.Precatorios.SDK.Entidades;

namespace UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database
{
    public interface IUnidadeDeTrabalho : IDisposable
    {
        IRepositorio<T> ObterRepositorio<T>() where T : class;

        Task<int> SalveAlteracoes();

        Task InicieTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null);

        Task FinalizeTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null);

        Task RevertaTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null);
    }

}
