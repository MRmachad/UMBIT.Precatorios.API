using Microsoft.EntityFrameworkCore;
using UMBIT.Precatorios.SDK.Repositorio.EF;

namespace UMBIT.Precatorios.Infraestrutura.Contextos
{
    public class AppContexto : BaseContext<AppContexto>
    {
        public AppContexto(DbContextOptions<AppContexto> options) : base(options)
        {
        }

    }
}
