using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UMBIT.Precatorios.Contrato.Judicial;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;
using UMBIT.Precatorios.Infraestrutura.Repositorios;

namespace UMBIT.Precatorios.API.Controllers
{
    public class JudicialController : JudicialControllerBase
    {
        private readonly IRepositorioDataProcesso RepositorioDataProcesso;
        private readonly IServicoDeGerenciamentoProcesso ServicoDeGerenciamentoProcesso;
        public JudicialController(IServiceProvider serviceProvider, IRepositorioDataProcesso repositorioDataProcesso, IServicoDeGerenciamentoProcesso servicoDeGerenciamentoProcesso) : base(serviceProvider)
        {
            RepositorioDataProcesso = repositorioDataProcesso;
            this.ServicoDeGerenciamentoProcesso = servicoDeGerenciamentoProcesso;
        }

        public override async Task<IActionResult> CriarVinculosGerenciamentoProcesso([FromBody] CriarVinculosGerenciamentoProcessoRequest request)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this.ServicoDeGerenciamentoProcesso.MarcarTodosComoLido(request.Uuids);
            });
        }

        public async override Task<IActionResult> ObterDataProcesso()
        {
            return await MiddlewareDeRetorno(async () =>
            {
                return this.RepositorioDataProcesso.Query();
            });
        }


        public async override Task<IActionResult> ObterVinculosGerenciamentoProcesso()
        {
            return await MiddlewareDeRetorno(async () =>
            {
                if (await this.ServicoDeGerenciamentoProcesso.Query().AnyAsync())
                {
                    return this.ServicoDeGerenciamentoProcesso.Query();
                }
                return null;
            });
        }
    }
}
