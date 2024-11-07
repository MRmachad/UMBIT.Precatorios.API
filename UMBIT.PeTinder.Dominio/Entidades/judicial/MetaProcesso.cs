using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UMBIT.Precatorios.Dominio.Entidades.judicial
{
    public class MetaProcesso
    {
        [Key]
        [Column("uuid")]
        public Guid Uuid { get; set; }

        [Column("NumeroProcesso")]
        [Required]
        [MaxLength(255)]
        public string NumeroProcesso { get; set; } = string.Empty;

        [Column("NumeroProcessoConsulta")]
        [Required]
        [MaxLength(255)]
        public string NumeroProcessoConsulta { get; set; } = string.Empty;

        [Column("Tipo")]
        [Required]
        [MaxLength(255)]
        public string Tipo { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Column("DataPublicacao")]
        [Required]
        public DateTime DataPublicacao { get; set; } = DateTime.Now;
    }
}
