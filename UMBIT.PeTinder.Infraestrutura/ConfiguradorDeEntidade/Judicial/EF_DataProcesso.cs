using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Precatorios.Dominio.Entidades.processo;

namespace UMBIT.Precatorios.Infraestrutura.ConfiguradorDeEntidade.Judicial
{
    internal class EF_DataProcesso : IEntityTypeConfiguration<DataProcesso>
    {
        public void Configure(EntityTypeBuilder<DataProcesso> builder)
        {
            builder.ToTable("processo");
            builder.HasKey(t => t.Uuid);
            builder.HasOne(p => p.MetaProcesso)
            .WithMany()
            .HasForeignKey(p => p.MetaProcessoId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
