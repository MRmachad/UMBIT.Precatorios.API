using System;

namespace UMBIT.Precatorios.Dominio.Entidades.auth.Permissao
{
    [Serializable]
    public class VinculoDePermissaoUsuario : VinculoDePermissaoBase
    {
        public Guid IdUsuario { get; set; }
    }
}
