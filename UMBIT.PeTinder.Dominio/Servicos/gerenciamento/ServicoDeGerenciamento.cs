using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Precatorios.Dominio.Entidades.judicial.gerenciamento;
using UMBIT.Precatorios.Dominio.Interfaces.Servicos;
using UMBIT.Precatorios.SDK.Repositorio.Interfaces.Database;
using UMBIT.Precatorios.SDK.Repositorio.Servicos;

namespace UMBIT.Precatorios.Dominio.Servicos.gerenciamento
{
    public class ServicoDeGerenciamento : ServicoBase<VinculoDeGerenciamentoProcesso>, IServicoDeGerenciamentoProcesso
    {
        public ServicoDeGerenciamento(IUnidadeDeTrabalho unidadeDeTrabalho) : base(unidadeDeTrabalho)
        {
        }

        public async Task MarcarTodosComoLido(List<string> uuids)
        {
            if (uuids != null && uuids.Any())
            {
                var vinculosExistentes = await this.Repositorio.Query().Where(t => uuids.Contains(t.Uuid)).ToListAsync();

                var vinculosLimpos = uuids.Where(t => !vinculosExistentes.Select(t => t.Uuid).Contains(t));

                if (vinculosLimpos.Any())
                {
                    foreach (var item in vinculosLimpos)
                    {
                        await this.Repositorio.Adicionar(new VinculoDeGerenciamentoProcesso(item));
                    }

                    await this.UnidadeDeTrabalho.SalveAlteracoes();
                }

            }
        }

    }
}
