
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Runtime.CompilerServices;
using UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database;
using  UMBIT.Precatorios.SDK.Entidades;

namespace UMBIT.Precatorios.SDK.Repositorio.EF
{
    public class UnidadeDeTrabalho : IUnidadeDeTrabalho
    {
        private DbContext _contexto { get; set; }
        private IServiceProvider _serviceProvider { get; set; }
        private IDbContextTransaction _transacao { get; set; }
        private bool _transacaoAberta { get; set; }

        public UnidadeDeTrabalho(DbContext contexto, IServiceProvider serviceProvider)
        {
            _contexto = contexto;
            _serviceProvider = serviceProvider;
        }

        public IRepositorio<T> ObterRepositorio<T>() where T : class
        {
            return _serviceProvider.GetService(typeof(IRepositorio<T>)) as IRepositorio<T> ?? new Repositorio<T>(this._contexto);
        }

        public async Task<int> SalveAlteracoes()
        {
            return await _contexto.SaveChangesAsync();
        }

        public async Task InicieTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null)
        {
            if (!_transacaoAberta)
            {
                _transacao = await _contexto.Database.BeginTransactionAsync();
                _transacaoAberta = true;
            }
        }

        public async Task FinalizeTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null)
        {
            if (_transacaoAberta)
            {
                await _transacao.CommitAsync();
                await _transacao.DisposeAsync();
                _transacaoAberta = false;
            }
        }

        public async Task RevertaTransacao([CallerFilePath] string arquivo = null, [CallerMemberName] string metodo = null)
        {
            if (_transacaoAberta)
            {
                await _transacao.RollbackAsync();
                await _transacao.DisposeAsync();
                _transacaoAberta = false;
            }
        }

        public void Dispose()
        {
            if (_transacao != null)
            {
                _transacao.Dispose();
            }

            _contexto.Dispose();
        }
    }
}
