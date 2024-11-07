using UMBIT.Precatorios.Dominio.Entidades.Basicos;

namespace UMBIT.Precatorios.Dominio.Interfaces.Servicos
{
    public interface IServicoDeUsuario<T> where T : Usuario
    {
        Task RemovaUsuario(Guid id);
        Task<Usuario> ObtenhaUsuario(Guid id);
        Task<Usuario> ObtenhaUsuario(string login);
        Task<IEnumerable<Usuario>> ObtenhaTodosOsUsuarios();
        Task RemoverPermissao(Guid id, string assemblyPermissao, bool reloadImediato = false);
        Task AdicionarPermissao(Guid id, string assemblyPermissao, bool reloadImediato = false);
    }
}
