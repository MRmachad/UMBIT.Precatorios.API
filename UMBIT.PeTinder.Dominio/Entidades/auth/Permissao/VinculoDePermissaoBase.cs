using UMBIT.Precatorios.SDK.Entidades;

namespace UMBIT.Precatorios.Dominio.Entidades.auth.Permissao
{
    [Serializable]
    public class VinculoDePermissaoBase : BaseEntity
    {
        public string Api { get; set; }
        public string TipoPermissao { get; set; }
        public int IdentificadorDePermissao { get; set; }

        public VinculoDePermissaoBase()
        {

        }
    }
}
