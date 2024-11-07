using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Precatorios.Dominio.Entidades.judicial;
using UMBIT.Precatorios.Dominio.Entidades.processo;

namespace UMBIT.Precatorios.Infraestrutura.ConfiguradorDeEntidade.Judicial
{
    internal class EF_MetaProcesso : IEntityTypeConfiguration<MetaProcesso>
    {
        public void Configure(EntityTypeBuilder<MetaProcesso> builder)
        {
            builder.ToTable("metaProcesso");
            builder.HasKey(t => t.Uuid);
        }
    }
}
