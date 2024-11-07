using UMBIT.Precatorios.Dominio.Entidades.auth.Permissao;
using UMBIT.Precatorios.SDK.Repositorio.EF;

namespace UMBIT.Precatorios.Auth.Infraestrutura.ConfiguracaoDeEntidades
{
    public class EF_VinculoDePermissaoUsuario : CoreEntityConfigurate<VinculoDePermissaoUsuario>
    {

        public override void ConfigureEntidade(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<VinculoDePermissaoUsuario> builder)
        {
          builder. HasKey(vinculo => new { vinculo.IdUsuario, vinculo.Api, vinculo.TipoPermissao, vinculo.IdentificadorDePermissao });
        }
    }
}
