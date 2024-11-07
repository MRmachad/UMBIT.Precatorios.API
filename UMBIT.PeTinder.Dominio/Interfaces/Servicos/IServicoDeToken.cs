using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Precatorios.Dominio.Entidades.auth.Token;

namespace UMBIT.Precatorios.Dominio.Interfaces.Servicos
{
    public interface IServicoDeToken
    {
        Task DeleteTokensDeUsuario(Guid idusuario);
        Task<IList<string>> ObtenhaChaves(string kid);
        Task<TokenResult> GereApiToken(Guid idUsuario, string audience);
    }
}
