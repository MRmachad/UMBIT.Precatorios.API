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
        [MaxLength(255)]
        public string? NumeroProcesso { get; set; }

        [Column("Classe")]
        [MaxLength(255)]
        public string? Classe { get; set; }

        [Column("NomePoloPassivo")]
        [MaxLength(255)]
        public string? NomePoloPassivo { get; set; }

        [Column("NomePoloAtivo")]
        [MaxLength(255)]
        public string? NomePoloAtivo { get; set; }

        [Column("Assunto")]
        [MaxLength(255)]
        public string? Assunto { get; set; }

        [Column("Valor")]
        [MaxLength(255)]
        public string? Valor { get; set; }

        [Column("Serventia")]
        [MaxLength(255)]
        public string? Serventia { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Column("NumeroProcessoConsulta")]
        [MaxLength(255)]
        public string? NumeroProcessoConsulta { get; set; }

        [Column("CpfCNPJPoloPassivo")]
        [MaxLength(255)]
        public string? CpfCNPJPoloPassivo { get; set; }

        [Column("CpfCNPJNomePoloAtivo")]
        [MaxLength(255)]
        public string? CpfCNPJNomePoloAtivo { get; set; }

        [Column("Serventia2")]
        [MaxLength(255)]
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
