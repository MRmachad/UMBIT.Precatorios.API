using UMBIT.Precatorios.SDK.Notificacao.Interfaces;

namespace UMBIT.Precatorios.SDK.Notificacao
{
    public class Notificador : INotificador
    {
        private readonly List<Notificacao> _notificacoes;
        private readonly List<ErroSistema> _errosSistema;


        public Notificador()
        {
            _notificacoes = new List<Notificacao>();
            _errosSistema = new List<ErroSistema>();    
        }

        public IEnumerable<Notificacao> ObterTodos()
        {
            var colecao = new List<Notificacao>();

            colecao.AddRange(_notificacoes);
            colecao.AddRange(_errosSistema);

            return colecao;
        }

        public IEnumerable<Notificacao> ObterNotificacoes()
            => _notificacoes;


        public IEnumerable<ErroSistema> ObterErrosSistema()
            => _errosSistema;

        public void AdicionarNotificacao(Notificacao notificacao)
            => _notificacoes.Add(notificacao);


        public void AdicionarErroSistema(ErroSistema erroSistema)
            => _errosSistema.Add(erroSistema);


        public bool TemNotificacoes()
        {
            if (_notificacoes.Count > 0 || _errosSistema.Count > 0)
                return true;

            return false;
        }

        public void LimparNotificacoes()
        {
            _notificacoes.Clear();
            _errosSistema.Clear();
        }
    }
}
