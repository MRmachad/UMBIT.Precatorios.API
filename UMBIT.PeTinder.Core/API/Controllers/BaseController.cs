using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using UMBIT.Precatorios.SDK.API.Models;
using UMBIT.Precatorios.SDK.Notificacao.Interfaces;

namespace UMBIT.Precatorios.SDK.API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected readonly INotificador Notificador;
        private readonly IHostingEnvironment _environment;

        public BaseController(IServiceProvider serviceProvider)
        {
            Notificador = serviceProvider.GetService<INotificador>();
            _environment = serviceProvider.GetService<IHostingEnvironment>();
        }

        protected virtual bool EhValido()
            => !Notificador.TemNotificacoes();

        protected virtual IActionResult Resposta(object resposta = null)
        {
            var result = RespostaDireta(resposta);

            if (EhValido())
                return Ok(result);

            return BadRequest(result);
        }
        protected virtual Resposta RespostaDireta(object resposta = null)
        {

            var dadosResposta = new Resposta();
            dadosResposta.Sucesso = EhValido();
            dadosResposta.Dados = resposta;
            dadosResposta.Erros = Notificador.ObterNotificacoes();

            if (_environment.IsDevelopment())
                dadosResposta.ErrosSistema = Notificador.ObterErrosSistema();

            return dadosResposta;
        }
        protected virtual ActionResult<T> Resposta<T>(object resposta = null) where T : class
        {

            var dadosResposta = new Resposta();
            dadosResposta.Sucesso = EhValido();
            dadosResposta.Dados = resposta;
            dadosResposta.Erros = Notificador.ObterNotificacoes();

            if (_environment.IsDevelopment())
                dadosResposta.ErrosSistema = Notificador.ObterErrosSistema();


            if (EhValido())
                return Ok(dadosResposta);

            return BadRequest(dadosResposta);
        }

        protected virtual IActionResult MiddlewareDeRetorno(Action func)
        {
            func();
            return Resposta();
        }
        protected virtual async Task<ActionResult<Resposta>> MiddlewareDeRetornoPadrao(Func<Task> func)
        {
            await func();
            return RespostaDireta();
        }
        protected virtual async Task<IActionResult> MiddlewareDeRetorno(Func<Task> func)
        {
            await func();
            return Resposta();
        }

        protected virtual IActionResult MiddlewareDeRetorno(Func<object?> func)
        {
            var res = func();
            return Resposta(res);
        }
        protected virtual ActionResult<T> MiddlewareDeRetorno<T>(Func<object?> func) where T : class
        {
            var res = func();
            return Resposta<T>(res);
        }
        protected virtual async Task<IActionResult> MiddlewareDeRetorno(Func<Task<object?>> func)
        {
            var res = await func();
            return Resposta(res);
        }
        protected virtual async Task<ActionResult<T>> MiddlewareDeRetorno<T>(Func<Task<object?>> func) where T : class
        {
            var res = await func();
            return Resposta<T>(res);
        }



    }
}
