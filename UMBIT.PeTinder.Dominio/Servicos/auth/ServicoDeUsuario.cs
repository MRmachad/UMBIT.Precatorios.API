using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;
using UMBIT.Precatorios.SDK.Notificacao;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;
using UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database;

namespace UMBIT.Precatorios.Dominio.Servicos.auth
{
    public class ServicoDeUsuario : IServicoDeUsuario<Usuario>
    {
        private readonly INotificador Notificador;
        private readonly IServicoDeToken ServicoDeToken;
        private readonly IUnidadeDeTrabalho UnidadeDeTrabalho;
        protected readonly UserManager<Usuario> UserManager;
        public ServicoDeUsuario(
            INotificador notificador,
            IServicoDeToken servicoDeToken,
            IUnidadeDeTrabalho unidadeDeTrabalho,
            UserManager<Usuario> userManager)
        {

            UserManager = userManager;
            Notificador = notificador;
            ServicoDeToken = servicoDeToken;
            UnidadeDeTrabalho = unidadeDeTrabalho;
        }

        public async Task AdicionarPermissao(Guid id, string assemblyPermissao, bool reloadImediato = false)
        {
            await UnidadeDeTrabalho.InicieTransacao();
            try
            {
                var usuario = await ObtenhaUsuario(id);

                var result = await UserManager.AddClaimAsync(usuario, new Claim(ClaimTypes.Role, assemblyPermissao));

                if (!result.Succeeded)
                    result.Errors.ToList().ForEach((t) => Notificador.AdicionarNotificacao(new Notificacao(t.Code, t.Description)));

                else if (reloadImediato)
                    await ServicoDeToken.DeleteTokensDeUsuario(id);

                await UnidadeDeTrabalho.FinalizeTransacao();
            }
            catch (Exception ex)
            {
                Notificador.AdicionarNotificacao(new ErroSistema("Erro ao adicionar Permissão", ex));
                await UnidadeDeTrabalho.RevertaTransacao();
            }
        }

        public async Task<IEnumerable<Usuario>> ObtenhaTodosOsUsuarios()
        {
            return await UserManager.Users.ToListAsync();
        }

        public async Task<Usuario> ObtenhaUsuario(Guid id)
        {
            return await UserManager.FindByIdAsync(id.ToString());
        }

        public async Task<Usuario> ObtenhaUsuario(string login)
        {
            return await UserManager.FindByEmailAsync(login);
        }

        public async Task RemovaUsuario(Guid id)
        {
            try
            {
                var usuario = await ObtenhaUsuario(id);

                if (usuario == null)
                {
                    Notificador.AdicionarNotificacao(new Notificacao("Falha ao remover Usuario", "Usuario não encontrado!"));
                    return;
                }

                await UserManager.DeleteAsync(usuario);
            }
            catch (Exception ex)
            {
                Notificador.AdicionarNotificacao(new ErroSistema("Erro ao remover Usuario", ex));
            }
        }

        public async Task RemoverPermissao(Guid id, string assemblyPermissao, bool reloadImediato = false)
        {
            await UnidadeDeTrabalho.InicieTransacao();
            try
            {
                var usuario = await ObtenhaUsuario(id);

                if (usuario == null)
                {
                    Notificador.AdicionarNotificacao(new Notificacao("Falha ao remover Permissão", "Usuario não encontrado!"));
                    return;
                }

                var claimToRemove = (await UserManager.GetClaimsAsync(usuario)).FirstOrDefault(t => t.Type == ClaimTypes.Role && t.Value == assemblyPermissao);

                if (claimToRemove != null)
                {
                    await UserManager.RemoveClaimAsync(usuario, claimToRemove);

                    if (reloadImediato)
                        await ServicoDeToken.DeleteTokensDeUsuario(id);
                    return;
                }

                Notificador.AdicionarNotificacao(new Notificacao("Falha ao remover Permissão", "Permissão não encontrada"));

                await UnidadeDeTrabalho.SalveAlteracoes();
                await UnidadeDeTrabalho.FinalizeTransacao();
            }
            catch (Exception ex)
            {
                Notificador.AdicionarNotificacao(new ErroSistema("Erro ao remover Permissão", ex));
                await UnidadeDeTrabalho.RevertaTransacao();
            }
        }
    }
}
