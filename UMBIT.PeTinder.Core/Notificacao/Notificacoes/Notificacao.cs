namespace UMBIT.Precatorios.SDK.Notificacao
{
    public class Notificacao
    {
        public string Titulo { get; protected set; } = string.Empty;
        public string Mensagem { get; protected set; } = string.Empty;

        public Notificacao(string mensagem)
        {
            Mensagem = mensagem;
        }

        public Notificacao(string titulo, string mensagem)
        {
            Titulo = titulo;
            Mensagem = mensagem;
        }
    }
}