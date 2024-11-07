using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UMBIT.Precatorios.Contrato.Cadastro;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;

namespace UMBIT.Precatorios.API.Controllers
{
    public class CadastroController : CadastroControllerBase
    {
        private readonly IServicoDeCadastro ServicoDeCadastro;
        public CadastroController(IServiceProvider serviceProvider, IServicoDeCadastro servicoDeCadastro) : base(serviceProvider)
        {
            this.ServicoDeCadastro = servicoDeCadastro;
        }

        public override async Task<IActionResult> Register([FromBody] Contrato.Cadastro.RegisterRequest body)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this.ServicoDeCadastro.Register(
                    body.Email,
                    body.Password,
                    body.Name,
                    body.FotoData,
                    body.DataNascimeto);
            });
        }
        public override async Task<IActionResult> ResetPassword([FromBody] Contrato.Cadastro.ResetPasswordRequest body)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this.ServicoDeCadastro.ResetPassword(body.Email, body.ResetCode, body.NewPassword);
            });
        }
        public override async Task<IActionResult> ForgotPassword([FromBody] Contrato.Cadastro.ForgotPasswordRequest body)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this.ServicoDeCadastro.ForgotPassword(body.Email);
            });
        }
        public override async Task<IActionResult> ResendConfirmationEmail([FromBody] Contrato.Cadastro.ResendConfirmationEmailRequest body)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this.ServicoDeCadastro.ResendConfirmationEmail(body.Email);
            });
        }
        public override async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code, [FromQuery] string changedEmail)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this.ServicoDeCadastro.ConfirmEmail(userId, code);
            });
        }

    }
}
