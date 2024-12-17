using System.ComponentModel.DataAnnotations.Schema;

namespace Questao5.Domain.Entities
{
    [Table("idempotencia")]
    public class Idempotencia
    {
        [Column("chave_idempotencia")]
        public string Chave_Idempotencia { get; set; }

        [Column("requisicao")]
        public string Requisicao { get; set; }

        [Column("resultado")]
        public string Resultado{ get; set; }
    }
}
