using UMBIT.Precatorios.Dominio.Entidades.auth.Permissao;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;

namespace UMBIT.Precatorios.Dominio.Interfaces.Servicos
{
    public interface IServicoDeAutorizacao
    {
        Task<IEnumerable<VinculoDePermissaoUsuario>> ObtenhaPermissoesDoUsuario(Guid idUsuario);
        Task RemovaPermissaoDoUsuario(Guid idUsuario, string api, string tipoPermissao, int identificador);
        Task AdicionePermissaoAoUsuario(Guid idUsuario, string api, string tipoPermissao, int identificador);
    }
}



