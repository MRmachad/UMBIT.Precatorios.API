using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Precatorios.Dominio.Entidades.processo;
using UMBIT.Precatorios.Infraestrutura.Contextos;

namespace UMBIT.Precatorios.Infraestrutura.Repositorios
{
    public interface IRepositorioDataProcesso
    {
        IQueryable<DataProcesso> Query();
    }
    public class RepositorioDataProcesso: IRepositorioDataProcesso
    {
        private JudicialContexto JudicialContexto;
        public RepositorioDataProcesso(JudicialContexto judicialContexto)
        {
            JudicialContexto = judicialContexto;
        }

        public IQueryable<DataProcesso> Query()
        {
            return JudicialContexto.Set<DataProcesso>().AsQueryable().Include(t => t.MetaProcesso);
        }
    }
}
