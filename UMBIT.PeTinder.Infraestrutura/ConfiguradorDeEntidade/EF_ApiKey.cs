using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UMBIT.Precatorios.Dominio.Entidades.auth.Token;
using UMBIT.Precatorios.SDK.Repositorio.EF;

namespace UMBIT.Precatorios.Infraestrutura.ConfiguracaoDeEntidades
{
    public class EF_ApiKey : CoreEntityConfigurate<ApiToken>
    {
        public override void ConfigureEntidade(EntityTypeBuilder<ApiToken> builder)
        {
        }
    }
}
