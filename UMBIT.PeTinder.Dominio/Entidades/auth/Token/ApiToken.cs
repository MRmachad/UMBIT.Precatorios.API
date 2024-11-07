using UMBIT.Precatorios.SDK.Entidades;

namespace UMBIT.Precatorios.Dominio.Entidades.auth.Token
{
    public class ApiToken : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid IdUsuario { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ExpiresMins { get; set; }
        public string ApiSecret { get; set; }
        public string Kid { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
