using System.Diagnostics;
using System.Runtime.CompilerServices;
using UMBIT.Precatorios.SDK.Basicos.Excecoes;

namespace UMBIT.Precatorios.SDK.Notificacao
{
    public class ErroSistema : Notificacao
    {
        public string Excecao { get; private set; } 
        public string ExcecaoInterna { get; private set; }
        public string RastreamentoPilha { get; private set; }
        public string MetodoCodigoFonte { get; private set; }
        public int LinhaCodigoFonte { get; private set; }
        public string NomeArquivoFonte { get; private set; }
        public object ObjetoManipulado { get; private set; }

        public ErroSistema(
            string mensagemErro, 
            [CallerMemberName] string metodoCodigoFonte = "",
            [CallerLineNumber] int linhaCodigoFonte = 0,
            [CallerFilePath] string nomeArquivoFonte = "") : base(mensagemErro)
        {
            Mensagem = mensagemErro;
            MetodoCodigoFonte = metodoCodigoFonte;
            LinhaCodigoFonte = linhaCodigoFonte;
            NomeArquivoFonte = nomeArquivoFonte;
        }

        public ErroSistema(
           string mensagemErro,
           object objetoManipulado,
           [CallerMemberName] string metodoCodigoFonte = "",
           [CallerLineNumber] int linhaCodigoFonte = 0,
           [CallerFilePath] string nomeArquivoFonte = "") : base(mensagemErro)
        {
            Mensagem = mensagemErro;
            ObjetoManipulado = objetoManipulado;
            MetodoCodigoFonte = metodoCodigoFonte;
            LinhaCodigoFonte = linhaCodigoFonte;
            NomeArquivoFonte = nomeArquivoFonte;
        }

        public ErroSistema(
            string titulo,
            string mensagemErro,
            object objetoManipulado,
            [CallerMemberName] string metodoCodigoFonte = "",
            [CallerLineNumber] int linhaCodigoFonte = 0,
            [CallerFilePath] string nomeArquivoFonte = "") : base(mensagemErro)
        {
            Titulo = titulo;
            Mensagem = mensagemErro;
            ObjetoManipulado = objetoManipulado;
            MetodoCodigoFonte = metodoCodigoFonte;
            LinhaCodigoFonte = linhaCodigoFonte;
            NomeArquivoFonte = nomeArquivoFonte;
        }

        public ErroSistema(
            string mensagemErro,
            Exception excecao) : base(mensagemErro)
        {
            Mensagem = mensagemErro;
            Excecao = excecao?.Message;
            ExcecaoInterna = excecao?.InnerException?.Message;
            RastreamentoPilha = excecao?.StackTrace;

            var st = new StackTrace(excecao, true);
            var frame = st.GetFrame(0);

            LinhaCodigoFonte = frame.GetFileLineNumber();
            NomeArquivoFonte = frame.GetFileName();
            MetodoCodigoFonte = excecao is ExcecaoBasicaUMBIT tseEx ? tseEx.MetodoCodigoFonte : frame.GetMethod().Name;
        }

        public ErroSistema(
            string mensagemErro,
            object objetoManipulado,
            Exception excecao) : base(mensagemErro)
        {
            Mensagem = mensagemErro;
            ObjetoManipulado = objetoManipulado;
            Excecao = excecao?.Message;
            ExcecaoInterna = excecao?.InnerException?.Message;
            RastreamentoPilha = excecao?.StackTrace;

            var st = new StackTrace(excecao, true);
            var frame = st.GetFrame(0);

            LinhaCodigoFonte = frame.GetFileLineNumber();
            NomeArquivoFonte = frame.GetFileName();
            MetodoCodigoFonte = excecao is ExcecaoBasicaUMBIT tseEx ? tseEx.MetodoCodigoFonte : frame.GetMethod().Name;
        }

        public ErroSistema(
            string titulo,
            string mensagemErro,
            Exception excecao) : base(mensagemErro)
        {
            Titulo = titulo;
            Mensagem = mensagemErro;
            Excecao = excecao?.Message;
            ExcecaoInterna = excecao?.InnerException?.Message;
            RastreamentoPilha = excecao?.StackTrace;

            var st = new StackTrace(excecao, true);
            var frame = st.GetFrame(0);

            LinhaCodigoFonte = frame.GetFileLineNumber();
            NomeArquivoFonte = frame.GetFileName();
            MetodoCodigoFonte = excecao is ExcecaoBasicaUMBIT tseEx ? tseEx.MetodoCodigoFonte : frame.GetMethod().Name;
        }

        public ErroSistema(
            string titulo,
            string mensagemErro,
            object objetoManipulado,
            Exception excecao) : base(mensagemErro)
        {
            Titulo = titulo;
            Mensagem = mensagemErro;
            ObjetoManipulado = objetoManipulado;
            Excecao = excecao?.Message;
            ExcecaoInterna = excecao?.InnerException?.Message;
            RastreamentoPilha = excecao?.StackTrace;


            var st = new StackTrace(excecao, true);
            var frame = st.GetFrame(0);

            LinhaCodigoFonte = frame.GetFileLineNumber();
            NomeArquivoFonte = frame.GetFileName();
            MetodoCodigoFonte = excecao is ExcecaoBasicaUMBIT tseEx ? tseEx.MetodoCodigoFonte : frame.GetMethod().Name;
        }


        public override string ToString()
            => $"{Mensagem} [Metodo: {MetodoCodigoFonte}; Linha: {LinhaCodigoFonte}; File: {NomeArquivoFonte}]";
        

    }
}
