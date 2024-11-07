using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UMBIT.Precatorios.Dominio.Configuradores;
using UMBIT.Precatorios.Dominio.Entidades.auth.Permissao;
using UMBIT.Precatorios.Dominio.Entidades.auth.Token;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;
using UMBIT.Precatorios.SDK.Basicos.Excecoes;
using UMBIT.Precatorios.SDK.Notificacao;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;
using UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database;
using UMBIT.Precatorios.SDK.Repositorio.Servicos;

namespace UMBIT.Precatorios.Dominio.Servicos.auth
{
    public class ServicoDeAutenticacao<T> : ServicoBase<T>, IServicoDeAutenticacao where T : Usuario
    {
        protected readonly UserManager<Usuario> UserManager;
        protected readonly SignInManager<Usuario> SignInManager;
        protected readonly IUserValidator<Usuario> UserValidator;
        private readonly IOptions<IdentitySettings> IdentitySettings;

        protected readonly INotificador Notificador;
        protected readonly IServicoDeToken ServicoDeToken;
        protected readonly IServicoDeUsuario<T> ServicoDeUsuario;
        protected readonly IRepositorio<VinculoDePermissaoUsuario> RepositorioDeVinculoDePermissao;

        public ServicoDeAutenticacao(
            IServicoDeToken servicoDeToken,
            IUnidadeDeTrabalho unidadeDeTrabalho,
            IServicoDeUsuario<T> servicoDeUsuario,
            SignInManager<Usuario> signInManager,
            UserManager<Usuario> userManage,
             IUserValidator<Usuario> userValidator,
             IOptions<IdentitySettings> identitySettings,
            INotificador notificador) :
            base(unidadeDeTrabalho)
        {
            Notificador = notificador;
            ServicoDeToken = servicoDeToken;
            ServicoDeUsuario = servicoDeUsuario;

            UserManager = userManage;
            SignInManager = signInManager;
            UserValidator = userValidator;
            IdentitySettings = identitySettings;

            RepositorioDeVinculoDePermissao = UnidadeDeTrabalho.ObterRepositorio<VinculoDePermissaoUsuario>();
        }

        public virtual async Task<bool> UsuarioAtivo(string usuario)
        {
            var _usuario = await UserManager.FindByEmailAsync(usuario);
            return await UserManager.IsLockedOutAsync(_usuario);
        }
        public virtual async Task<bool> ValideUsuario(string usuario, string senha)
        {
            var validator = await UserValidator.ValidateAsync(UserManager, new Usuario
            {
                Email = usuario,
                UserName = usuario,
                PasswordHash = senha,
            });

            return validator.Succeeded;
        }
        public virtual async Task<TokenResult> AutentiqueUsuario(string usuario, string senha, string audience)
        {
            try
            {
                var tokenResult = new TokenResult();

                var result = await SignInManager.PasswordSignInAsync(usuario,
                                                                         senha,
                                                                         isPersistent: false,
                                                                         lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    var _usuario = await UserManager.FindByEmailAsync(usuario);

                    if (_usuario.TwoFactorEnabled)
                    {
                        tokenResult.EnabledTwoFactor = true;
                        var tokenTwoFactor = await UserManager.GenerateTwoFactorTokenAsync(_usuario, "Email");
                        //envio de token por email 
                    }
                    else
                        return await GereToken(_usuario, audience);
                }
                else if (result.IsLockedOut)
                {
                    Notificador.AdicionarNotificacao(new Notificacao("Falha De login", "Usuario bloqueado temporariamente por tentantivas invalidadas"));
                }

                Notificador.AdicionarNotificacao(new Notificacao("Falha De login", "Usuario ou senha Incorretos"));

                return tokenResult;
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro na operação de Login", ex);
            }

        }

        public async Task<TokenResult> TwofactorLogin(string email, string twoFactorCode, string audience)
        {
            try
            {
                var tokenResult = new TokenResult();
                var _usuario = await UserManager.FindByEmailAsync(email);

                if (_usuario != null && (await SignInManager.TwoFactorSignInAsync("Email", twoFactorCode, false, false)).Succeeded)
                {
                    return await GereToken(_usuario, audience);
                }
                else if (_usuario != null)
                    Notificador.AdicionarNotificacao(new Notificacao("Falha De login", "Falha ao authenticar código!"));
                else
                    Notificador.AdicionarNotificacao(new Notificacao("Falha De login", "Usuario não encontrado!"));

                return tokenResult;
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro na operação de Login Two Factor", ex);
            }
        }

        public async Task<Usuario> ObtenhaUsuario(Guid id)
        {
            return await ServicoDeUsuario.ObtenhaUsuario(id);
        }
        public async Task<IList<string>> ObtenhaChaves(string kid)
        {
            return await ServicoDeToken.ObtenhaChaves(kid);
        }
        public async Task<IEnumerable<Usuario>> ObtenhaTodosOsUsuarios()
        {
            return await ServicoDeUsuario.ObtenhaTodosOsUsuarios();
        }
        public async Task<TokenResult> GereToken(Usuario usuario, string audience)
        {
            return await ServicoDeToken.GereApiToken(usuario.Id, audience);
        }

        public Task Deslogar()
        {

            return Task.CompletedTask;
        }

    }
}
