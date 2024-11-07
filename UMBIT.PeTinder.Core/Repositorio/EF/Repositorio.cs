using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database;

namespace UMBIT.Precatorios.SDK.Repositorio.EF
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        protected DbContext _contexto { get; private set; }
        protected DbSet<T> Db { get; private set; }


        public Repositorio(DbContext contexto)
        {
            _contexto = contexto;
            Db = _contexto.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> ObterTodos()
        {
            return await Db
                .AsNoTracking()
                .ToListAsync();
        }

        public virtual async Task<T> ObterUnico(params object[] args)
        {
            return Db.Find(args);
        }

        public virtual async Task<IEnumerable<T>> Filtrar(Expression<Func<T, bool>> predicado)
        {
            return await Db
                .AsNoTracking()
                .Where(predicado)
                .ToListAsync();
        }

        public virtual T Carregar(T objeto)
        {
            return Db.Attach(objeto) as T;
        }

        public virtual async Task Adicionar(T objeto)
        {
            await Db.AddAsync(objeto);
        }

        public virtual async Task Adicionar(IEnumerable<T> objetos)
        {
            await Db.AddRangeAsync(objetos);
        }

        public virtual void Atualizar(T objeto)
        {
            var result = Db.Update(objeto);
            _contexto.Entry(objeto).State = result.State;
        }

        public virtual void Remover(T objeto)
        {
            var result = Db.Remove(objeto);
            _contexto.Entry(objeto).State = result.State;
        }

        protected void MiddlewareDeRepositorio(Action method)
        {
            try
            {
                method();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro no processamento do banco de dados. Contate o administrador.", ex);
            }
        }

        protected TRes MiddlewareDeRepositorio<TRes>(Func<TRes> method)
        {
            try
            {
                return method();
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro no processamento do banco de dados. Contate o administrador.", ex);
            }
        }

        public IQueryable<T> Query()
        {
            return Db.AsQueryable<T>();
        }
    }
}
