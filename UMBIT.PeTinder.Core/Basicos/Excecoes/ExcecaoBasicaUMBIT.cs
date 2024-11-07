using System.Runtime.CompilerServices;

namespace UMBIT.Precatorios.SDK.Basicos.Excecoes
{
    public class ExcecaoBasicaUMBIT : Exception
    {
        public string Mensagem { get; private set; }
        public string MetodoCodigoFonte { get; private set; }
        public Exception ExcecaoInterna { get; private set; }
        public ExcecaoBasicaUMBIT(string mensagem) : base(mensagem)
        {
            this.Mensagem = mensagem;
        }
        public ExcecaoBasicaUMBIT(
            string mensagem,
            Exception ex,  
            [CallerMemberName] string metodoCodigoFonte = "") : base(mensagem, ex)
        {
            this.ExcecaoInterna = ex;
            this.MetodoCodigoFonte = metodoCodigoFonte;
            this.Mensagem = ex.GetType() == typeof(ExcecaoBasicaUMBIT) ? ((ExcecaoBasicaUMBIT)ex).Mensagem : mensagem;
        }

        public override string ToString()
        {
            return this.Mensagem;
        }
    }
}
