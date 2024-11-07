using UMBIT.Precatorios.SDK.Workers.Basicos.Enum;

namespace UMBIT.Precatorios.SDK.Workers.Basicos.Utilitarios
{
    public static class UtilitarioDeRecorrencia
    {
        public static string ObtenhaCron(Recorrencia recorrenciaServico)
        {
            switch (recorrenciaServico)
            {
                case Recorrencia.MIN_5:
                    return "*/5 * * * *";
                case Recorrencia.MIN_10:
                    return "*/10 * * * *";
                case Recorrencia.MIN_30:
                    return "*/30 * * * *";
                case Recorrencia.HORA:
                    return "0 * * * *";
                case Recorrencia.HORA_2:
                    return "0 */2 * * *";
                case Recorrencia.HORA_3:
                    return "0 */3 * * *";
                case Recorrencia.HORA_6:
                    return "0 */6 * * *";
                case Recorrencia.DIARIO:
                    return "0 0 * * *";
                default:
                    return "0 0 * * *";
            }
        }
        public static uint ObtenhaMinutos(Recorrencia recorrenciaServico)
        {
            switch (recorrenciaServico)
            {
                case Recorrencia.MIN_5:
                    return 5;
                case Recorrencia.MIN_10:
                    return 10;
                case Recorrencia.MIN_30:
                    return 30;
                case Recorrencia.HORA:
                    return 60;
                case Recorrencia.HORA_2:
                    return 60*2;
                case Recorrencia.HORA_3:
                    return 60 * 3;
                case Recorrencia.HORA_6:
                    return 60 * 6;
                case Recorrencia.DIARIO:
                    return 60 * 24;
                default:
                    return 1;
            }
        }

    }
}
