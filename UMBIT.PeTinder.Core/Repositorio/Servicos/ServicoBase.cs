using UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database;

namespace UMBIT.Precatorios.SDK.Repositorio.Servicos
{
    public abstract class ServicoBase<T> : IDisposable, IServicoBase<T> where T : class
    {
        protected readonly IUnidadeDeTrabalho UnidadeDeTrabalho;
        protected IRepositorio<T> Repositorio { get; private set; }

        public ServicoBase(IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            UnidadeDeTrabalho = unidadeDeTrabalho;
            Repositorio = UnidadeDeTrabalho.ObterRepositorio<T>();
        }

        public virtual async Task<IEnumerable<T>> Obter()
        {
            return await Repositorio.ObterTodos();
        }
        public virtual IQueryable<T> Query()
        {
            return Repositorio.Query();
        }

        public virtual async Task<T> ObterUnico(Guid id)
        {
            return await Repositorio.ObterUnico(id);
        }

        public virtual async Task Adicionar(T objeto)
        {
            await Repositorio.Adicionar(objeto);
            await UnidadeDeTrabalho.SalveAlteracoes();
        }

        public virtual async Task Adicionar(IEnumerable<T> objetos)
        {
            await Repositorio.Adicionar(objetos);
            await UnidadeDeTrabalho.SalveAlteracoes();
        }

        public virtual async Task Atualize(T objeto)
        {
            Repositorio.Atualizar(objeto);
            await UnidadeDeTrabalho.SalveAlteracoes();
        }

        public virtual async Task Remover(Guid id)
        {
            var objeto = await ObterUnico(id);

            Repositorio.Remover(objeto);
            await UnidadeDeTrabalho.SalveAlteracoes();
        }

        public void Dispose()
        {
            UnidadeDeTrabalho.Dispose();
        }
    }

    public abstract class ServicoBase : IDisposable
    {
        protected readonly IUnidadeDeTrabalho UnidadeDeTrabalho;

        public ServicoBase(IUnidadeDeTrabalho unidadeDeTrabalho)
        {
            UnidadeDeTrabalho = unidadeDeTrabalho;
        }

        public void Dispose()
        {
            UnidadeDeTrabalho.Dispose();
        }
    }
}
