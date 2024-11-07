using UMBIT.Precatorios.Dominio.Entidades.auth.Token;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;

namespace UMBIT.Precatorios.Dominio.Interfaces.Servicos
{
    public interface IServicoDeAutenticacao
    {

        Task<bool> UsuarioAtivo(string user);
        Task<IList<string>> ObtenhaChaves(string kid);
        Task<bool> ValideUsuario(string user, string senha);
        Task<TokenResult> AutentiqueUsuario(string user, string senha, string audience);
        Task<TokenResult> TwofactorLogin(string email, string twoFactorCode, string audience);

        Task<Usuario> ObtenhaUsuario(Guid id);
        Task<IEnumerable<Usuario>> ObtenhaTodosOsUsuarios();

        Task Deslogar();
        Task<TokenResult> GereToken(Usuario usuario, string audience);
    }
}
