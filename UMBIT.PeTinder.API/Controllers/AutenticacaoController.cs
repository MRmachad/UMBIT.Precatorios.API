using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UMBIT.Precatorios.Contrato.Autenticacao;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;

namespace UMBIT.Precatorios.API.Controllers
{
    public class AutenticacaoController : AutenticacaoControllerBase
    {
        private readonly IServicoDeAutenticacao ServicoDeAutenticacao;

        public AutenticacaoController(IServiceProvider serviceProvider, IServicoDeAutenticacao servicoDeAutenticacao) : base(serviceProvider)
        {
            this.ServicoDeAutenticacao = servicoDeAutenticacao;
        }

        public override Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }
        public override async Task<ActionResult<TokenResponse>> Login([FromBody] Login login)
        {
            return await MiddlewareDeRetorno<TokenResponse>(async () =>
            {
                return await this.ServicoDeAutenticacao.AutentiqueUsuario(login.User, login.Password, login.Audience);
            });
        }

        public override  async Task<ActionResult<TokenResponse>> Twofactor([FromBody] TwoFactorRequest body)
        {
            return await MiddlewareDeRetorno<TokenResponse>(async () =>
            {
                return await this.ServicoDeAutenticacao.TwofactorLogin(body.Email, body.TwoFactorCode, body.Audience);
            });
        }
    }
}
