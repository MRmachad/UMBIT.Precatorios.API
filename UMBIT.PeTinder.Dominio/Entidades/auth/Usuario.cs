using Microsoft.AspNetCore.Identity;

namespace UMBIT.Precatorios.Dominio.Entidades.Basicos
{
    public class Usuario: IdentityUser<Guid>
    {
        public string? Nome { get;  set; }

        public virtual string ObtenhaIdentificador()
        {
            return $"{Nome} ({UserName})";
        }
    }
}
