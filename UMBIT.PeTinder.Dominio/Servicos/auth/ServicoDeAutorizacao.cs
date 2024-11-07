using UMBIT.Precatorios.Dominio.Entidades.auth.Permissao;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;
using UMBIT.Precatorios.SDK.Basicos.Excecoes;
using UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database;
using UMBIT.Precatorios.SDK.Repositorio.Servicos;

namespace UMBIT.Precatorios.Dominio.Servicos.auth
{
    public class ServicoDeAutorizacao : ServicoBase<Usuario>, IServicoDeAutorizacao
    {
        private IRepositorio<Usuario> RepositorioDeUsuario { get; set; }
        private IRepositorio<VinculoDePermissaoUsuario> RepositorioDePermissoesDoUsuario { get; set; }


        public ServicoDeAutorizacao(IUnidadeDeTrabalho unidadeDeTrabalho) : base(unidadeDeTrabalho)
        {
            RepositorioDeUsuario = UnidadeDeTrabalho.ObterRepositorio<Usuario>();
            RepositorioDePermissoesDoUsuario = UnidadeDeTrabalho.ObterRepositorio<VinculoDePermissaoUsuario>();
        }

        public async Task<IEnumerable<VinculoDePermissaoUsuario>> ObtenhaPermissoesDoUsuario(Guid idUsuario)
        {
            var res = await RepositorioDePermissoesDoUsuario.Filtrar(permissao => permissao.IdUsuario == idUsuario);

            return res.Select(vinc => vinc);
        }
        public virtual async Task RemovaPermissaoDoUsuario(Guid idUsuario, string api, string tipoPermissao, int identificador)
        {
            await UnidadeDeTrabalho.InicieTransacao();

            try
            {
                var usuario = RepositorioDeUsuario.ObterUnico(idUsuario);

                var vinculoPermissao = await RepositorioDePermissoesDoUsuario.ObterUnico(idUsuario, api, tipoPermissao, identificador);

                if (vinculoPermissao != null)
                {
                    RepositorioDePermissoesDoUsuario.Remover(vinculoPermissao);
                    await UnidadeDeTrabalho.SalveAlteracoes();

                }

                await UnidadeDeTrabalho.FinalizeTransacao();
            }
            catch (Exception ex)
            {
                await UnidadeDeTrabalho.RevertaTransacao();
                throw new ExcecaoBasicaUMBIT("Falha ao remover permissão para o usuário", ex);
            }
        }
        public virtual async Task AdicionePermissaoAoUsuario(Guid idUsuario, string api, string tipoPermissao, int identificador)
        {
            await UnidadeDeTrabalho.InicieTransacao();

            try
            {
                var usuario = RepositorioDeUsuario.ObterUnico(idUsuario);

                var vinculoPermissao = new VinculoDePermissaoUsuario();

                vinculoPermissao.Api = api;
                vinculoPermissao.IdUsuario = idUsuario;
                vinculoPermissao.TipoPermissao = tipoPermissao;
                vinculoPermissao.IdentificadorDePermissao = identificador;

                await RepositorioDePermissoesDoUsuario.Adicionar(vinculoPermissao);

                await UnidadeDeTrabalho.SalveAlteracoes();
                await UnidadeDeTrabalho.FinalizeTransacao();
            }
            catch (Exception ex)
            {
                await UnidadeDeTrabalho.RevertaTransacao();
                throw new ExcecaoBasicaUMBIT("Falha ao adicionar permissão para o usuário", ex);
            }
        }

    }
}
