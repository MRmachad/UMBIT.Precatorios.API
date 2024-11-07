using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UMBIT.Precatorios.Dominio.Entidades.judicial.gerenciamento;
using UMBIT.Precatorios.SDK.Repositorio.EF;

namespace UMBIT.Precatorios.Infraestrutura.ConfiguradorDeEntidade
{
    class EF_VinculoDeGerenciamentoProcesso : CoreEntityConfigurate<VinculoDeGerenciamentoProcesso>
    {
        public override void ConfigureEntidade(EntityTypeBuilder<VinculoDeGerenciamentoProcesso> builder)
        {
            builder.HasIndex(t => t.Uuid);
        }
    }
}
