using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMBIT.Precatorios.Dominio.Entidades.auth
{
    public class AuthenticationSettings
    {
        public string Issuer { get; set; }

        public string BaseAddress { get; set; }

        public double ExpiresMins { get; set; }

        public string[] Audiences { get; set; }

        public string ChallengeScheme { get; set; }

        public string AuthenticateScheme { get; set; }
    }
}
