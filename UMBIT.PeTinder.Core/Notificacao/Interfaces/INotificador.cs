namespace UMBIT.Precatorios.SDK.Notificacao.Interfaces
{
    public interface INotificador 
    {
        IEnumerable<Notificacao> ObterTodos(); 
        IEnumerable<Notificacao> ObterNotificacoes();
        IEnumerable<ErroSistema> ObterErrosSistema();
        void AdicionarNotificacao(Notificacao notificacao);
        void AdicionarErroSistema(ErroSistema erroSistema);
        bool TemNotificacoes();
        void LimparNotificacoes();
    }
}
