using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;

namespace UMBIT.Precatorios.Infraestrutura.Contexto
{
    public class IdentityContext : IdentityDbContext<Usuario, Role, Guid>
    {
        public IdentityContext(DbContextOptions<IdentityContext> option) : base(option)
        {

        }
    }
}
