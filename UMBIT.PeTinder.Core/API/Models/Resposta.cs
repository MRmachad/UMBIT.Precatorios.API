using System.Text.Json.Serialization;
using UMBIT.Precatorios.SDK.Notificacao;

namespace UMBIT.Precatorios.SDK.API.Models
{
    public class Resposta
    {
        [JsonPropertyName("sucesso")]
        public bool Sucesso { get; set; }

        [JsonPropertyName("dados")]
        public object Dados { get; set; }

        [JsonPropertyName("erros")]
        public IEnumerable<Notificacao.Notificacao> Erros { get; set; }

        [JsonPropertyName("erros_sistema")]
        public IEnumerable<ErroSistema> ErrosSistema { get; set; }
    }
}
