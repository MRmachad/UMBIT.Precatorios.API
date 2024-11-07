using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;
using UMBIT.Precatorios.SDK.Basicos.Excecoes;
using UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database;
using UMBIT.Precatorios.Dominio.Entidades.auth.Token;
using UMBIT.Precatorios.Dominio.Entidades.auth;

namespace UMBIT.Precatorios.Dominio.Servicos.auth
{
    public class ServicoDeToken : IServicoDeToken
    {
        private readonly IUnidadeDeTrabalho UnidadeDeTrabalho;
        private readonly IRepositorio<ApiToken> RepositorioDeToken;
        private readonly IOptions<AuthenticationSettings> TokenSettings;

        protected readonly UserManager<Usuario> UserManager;

        public ServicoDeToken(
            UserManager<Usuario> userManager,
            IUnidadeDeTrabalho unidadeDeTrabalho,
            IOptions<AuthenticationSettings> tokenSettings
            )
        {
            UserManager = userManager;
            TokenSettings = tokenSettings;
            UnidadeDeTrabalho = unidadeDeTrabalho;
            RepositorioDeToken = unidadeDeTrabalho.ObterRepositorio<ApiToken>();
        }

        private string GereKid()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
        private string GereApiSecret()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1910, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        private async Task<ClaimsIdentity> ObtenhaClaims(Guid idUser)
        {
            var identityClaims = new ClaimsIdentity();

            var user = await UserManager.FindByIdAsync(idUser.ToString());
            var userClaims = await UserManager.GetClaimsAsync(user);
            identityClaims.AddClaims(userClaims);


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, idUser.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Name, user.Nome ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64)
            };

            identityClaims.AddClaims(claims);

            return await Task.FromResult(identityClaims);
        }
        private string ObtenhaToken(ClaimsIdentity identityClaims, string secret, string kid, double expiracaoMins, string audience, string emissor)
        {
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = emissor,
                Audience = audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddMinutes(expiracaoMins),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key) { KeyId = kid }, SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
        private TokenResult ObtenhaRespostaToken(Guid idUser, string encodedToken, string encodedRefreshToken, double expiracaoMins, ClaimsIdentity claimsIdentity)
        {
            var response = new TokenResult
            {
                AccessToken = encodedToken,
                RefreshToken = encodedRefreshToken,
                ExpiresIn = TimeSpan.FromMinutes(expiracaoMins).TotalSeconds,
                UsuarioToken = new UsuarioToken
                {
                    Id = idUser.ToString(),
                    Nome = claimsIdentity.FindFirst(JwtRegisteredClaimNames.Name)?.Value,
                    Claims = claimsIdentity.Claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }

        public async Task DeleteTokensDeUsuario(Guid idusuario)
        {
            var tokensUsuario = await RepositorioDeToken.ObterTodos();
            foreach (var item in tokensUsuario.Where(t => t.IdUsuario == idusuario))
            {
                RepositorioDeToken.Remover(item);
                await UnidadeDeTrabalho.SalveAlteracoes();
            }
        }
        public async Task<IList<string>> ObtenhaChaves(string kid)
        {
            var securityKeys = new List<string>();
            var apiTokens = await RepositorioDeToken.ObterTodos();

            foreach (var t in apiTokens.Where(t => t.Kid == kid))
                securityKeys.Add(t.ApiSecret);

            return securityKeys;
        }
        public async Task<TokenResult> GereApiToken(Guid idusuario, string audience)
        {
            await UnidadeDeTrabalho.InicieTransacao();
            try
            {
                await DeleteTokensDeUsuario(idusuario);

                var token = new ApiToken
                {
                    Kid = GereKid(),
                    Audience = audience,
                    IdUsuario = idusuario,
                    ApiSecret = GereApiSecret(),
                    Issuer = TokenSettings.Value.Issuer,
                    ExpiresMins = TokenSettings.Value.ExpiresMins,
                };

                var identityClaims = await ObtenhaClaims(token.IdUsuario);

                var encodedToken = ObtenhaToken(identityClaims, token.ApiSecret, token.Kid, token.ExpiresMins, token.Audience, token.Issuer);
                var encodedRefreshToken = ObtenhaToken(null, token.ApiSecret, token.Kid, token.ExpiresMins * 3, token.Audience, token.Issuer);

                var tokenResult = ObtenhaRespostaToken(token.IdUsuario, encodedToken, encodedRefreshToken, token.ExpiresMins, identityClaims);

                token.AccessToken = tokenResult.AccessToken;
                token.RefreshToken = tokenResult.RefreshToken;

                await RepositorioDeToken.Adicionar(token);
                await UnidadeDeTrabalho.SalveAlteracoes();

                await UnidadeDeTrabalho.FinalizeTransacao();

                return tokenResult;
            }
            catch (Exception ex)
            {

                await UnidadeDeTrabalho.RevertaTransacao();
                throw new ExcecaoBasicaUMBIT("Falha ao gerar token!", ex);
            }


        }

    }
}
