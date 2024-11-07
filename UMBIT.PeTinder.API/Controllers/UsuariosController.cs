using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UMBIT.Precatorios.Contrato.Usuarios;
using UMBIT.Precatorios.Dominio.Entidades.Basicos;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;

namespace UMBIT.Precatorios.API.Controllers
{
    public class UsuariosController : UsuariosControllerBase
    {
        private readonly IServicoDeUsuario<Usuario> ServicoDeUsuario;
        public UsuariosController(IServiceProvider serviceProvider, IServicoDeUsuario<Usuario> servicoDeUsuario) : base(serviceProvider)
        {
            this.ServicoDeUsuario = servicoDeUsuario;
        }
        public override async Task<IActionResult> AdicionarPermissao([FromBody] PermissaoRequest body)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this.ServicoDeUsuario.AdicionarPermissao(Guid.Parse(body.Id), body.AssemblyPermissao, body.ReloadImediato);
            });
        }

        public override async Task<IActionResult> ObtenhaTodos()
        {
            return await MiddlewareDeRetorno(async () =>
            {
                return await this.ServicoDeUsuario.ObtenhaTodosOsUsuarios();
            });
        }

        public override async Task<IActionResult> RemoverPermissao([FromBody] PermissaoRequest body)
        {

            return await MiddlewareDeRetorno(async () =>
            {
                await this.ServicoDeUsuario.RemoverPermissao(Guid.Parse(body.Id), body.AssemblyPermissao, body.ReloadImediato);
            });
        }


        public override async Task<IActionResult> RemoverUsuario(string id)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this.ServicoDeUsuario.RemovaUsuario(Guid.Parse(id));
            });
        }
    }
}
