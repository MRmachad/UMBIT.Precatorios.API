using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.Precatorios.Dominio.Entidades.judicial;

namespace UMBIT.Precatorios.Dominio.Entidades.processo
{
    public class DataProcesso
    {
        [Key]
        [Column("uuid")]
        public Guid Uuid { get; set; }

        [Column("NumeroProcesso")]
        public string? NumeroProcesso { get; set; }

        [Column("Classe")]
        public string? Classe { get; set; }

        [Column("NomePoloPassivo")]
        public string? NomePoloPassivo { get; set; }

        [Column("NomePoloAtivo")]
        public string? NomePoloAtivo { get; set; }

        [Column("Assunto")]
        public string? Assunto { get; set; }

        [Column("Valor")]
        public float Valor { get; set; }

        [Column("Serventia")]
        public string? Serventia { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Column("NumeroProcessoConsulta")]
        public string? NumeroProcessoConsulta { get; set; }

        [Column("CpfCNPJPoloPassivo")]
        public string? CpfCNPJPoloPassivo { get; set; }

        [Column("CpfCNPJNomePoloAtivo")]
        public string? CpfCNPJNomePoloAtivo { get; set; }

        [Column("Serventia2")]
        public string? Serventia2 { get; set; }

        [ForeignKey("metaProcesso")]
        [Column("meta_processo_id")]
        public Guid MetaProcessoId { get; set; }

        public virtual MetaProcesso MetaProcesso { get; set; }

        public DataProcesso()
        {

        }
    }
}
