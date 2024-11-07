using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMBIT.Precatorios.Dominio.Entidades.auth.Token
{
    public class TokenResult
    {
        public bool EnabledTwoFactor { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken UsuarioToken { get; set; }

    }
    public class UsuarioToken
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public IEnumerable<UsuarioClaim> Claims { get; set; }
    }
    public class UsuarioClaim
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
