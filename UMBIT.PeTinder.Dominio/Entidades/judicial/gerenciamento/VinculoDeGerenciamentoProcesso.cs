using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Precatorios.SDK.Entidades;

namespace UMBIT.Precatorios.Dominio.Entidades.judicial.gerenciamento
{
    public class VinculoDeGerenciamentoProcesso : BaseEntity
    {
        public VinculoDeGerenciamentoProcesso()
        {

        }
        public VinculoDeGerenciamentoProcesso(string uuid)
        {
            Uuid = uuid;
        }

        [Column("uuid")]
        public string Uuid { get; set; }

        [Column("status")]
        public int Status { get; set; }
    }
}
