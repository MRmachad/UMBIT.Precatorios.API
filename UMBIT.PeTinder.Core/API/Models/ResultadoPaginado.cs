
namespace TSE.Nexus.SDK.API.Models
{
    public class ResultadoPaginado<T> where T : class
    {
        public int Pagina { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalItens { get; set; }
        public int TotalPaginas => (int)Math.Ceiling(TotalItens / (double)TamanhoPagina);
        public IEnumerable<T> Data { get; set; }
    }
}
