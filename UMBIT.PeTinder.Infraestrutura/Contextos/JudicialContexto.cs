using Microsoft.EntityFrameworkCore;
using UMBIT.Precatorios.Infraestrutura.ConfiguradorDeEntidade.Judicial;

namespace UMBIT.Precatorios.Infraestrutura.Contextos
{
    public class JudicialContexto : DbContext
    {
        public JudicialContexto(DbContextOptions<JudicialContexto> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            dynamic instanciaDeConfiguracao = Activator.CreateInstance(typeof(EF_DataProcesso))!;
            dynamic instanciaDeConfiguracao2 = Activator.CreateInstance(typeof(EF_MetaProcesso))!;
            modelBuilder.ApplyConfiguration(instanciaDeConfiguracao2);
            modelBuilder.ApplyConfiguration(instanciaDeConfiguracao);

            base.OnModelCreating(modelBuilder);
        }


    }
}
