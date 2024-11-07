using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Precatorios.Dominio.Entidades.judicial.gerenciamento;
using UMBIT.Precatorios.SDK.Repositorio.Servicos;

namespace UMBIT.Precatorios.Dominio.Interfaces.Servicos
{
    public interface IServicoDeGerenciamentoProcesso: IServicoBase<VinculoDeGerenciamentoProcesso>
    {
        Task MarcarTodosComoLido(List<string> uuids);
    }
}
