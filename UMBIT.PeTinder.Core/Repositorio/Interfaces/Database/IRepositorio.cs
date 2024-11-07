using System.Linq.Expressions;

namespace UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database
{
    public interface IRepositorio<T> where T : class
    {
        IQueryable<T> Query();

        /// <summary>
        /// Obtenha Todos os Registros
        /// </summary>
        /// <returns>Lista de Registros</returns>
        Task<IEnumerable<T>> ObterTodos();

        /// <summary>
        /// Obtenha objeto único a partir da chave primária
        /// </summary>
        /// <param name="args">Chave primária</param>
        /// <returns>Objeto único</returns>
        Task<T> ObterUnico(params object[] args);

        /// <summary>
        /// Filtra objetos
        /// </summary>
        /// <param name="predicado">Predicado de filtragem</param>
        /// <returns>Lista de Registros filtrados</returns>
        Task<IEnumerable<T>> Filtrar(Expression<Func<T, bool>> predicado);

        /// <summary>
        /// Obtenha objeto único a partir da chave primária
        /// </summary>
        /// <param name="args">Chave primária</param>
        /// <returns>Objeto único</returns>
        T Carregar(T Objeto);

        /// <summary>
        /// Adiciona objeto na Base de Dados
        /// </summary>
        /// <param name="objeto">Objeto a ser adicionado</param>
        Task Adicionar(T objeto);

        /// <summary>
        /// Adiciona objetos na Base de Dados
        /// </summary>
        /// <param name="objeto">Objetos a serem adicionados</param>
        Task Adicionar(IEnumerable<T> objetos);

        /// <summary>
        /// Atualiza objeto na Base de Dados
        /// </summary>
        /// <param name="objeto">Objeto a ser atualizado</param>
        void Atualizar(T objeto);

        /// <summary>
        /// Remova objeto da base de dados
        /// </summary>
        /// <param name="objeto">Objeto a ser removido</param>
        void Remover(T objeto);
    }
}