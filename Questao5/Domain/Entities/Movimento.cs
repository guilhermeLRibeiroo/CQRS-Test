using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
    [Table("movimento")]
    public class Movimento
    { 
        [Column("idmovimento")]
        public string IdMovimento { get; set; }

        [Column("idcontacorrente")]
        public string IdContaCorrente { get; set; }

        [Column("datamovimento")]
        public string DataMovimento { get; set; }

        [Column("tipomovimento")]
        public string TipoMovimento { get; set; }

        [Column("valor")]
        public decimal Valor { get; set; }
    }
}
