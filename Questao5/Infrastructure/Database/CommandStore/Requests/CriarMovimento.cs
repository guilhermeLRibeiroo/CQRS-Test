using System.Text.Json;

namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class CriarMovimento
    {
        public Guid IdempotenciaId { get; set; }
        public Guid MovimentoId { get; set; }
        public Guid ContaCorrenteId { get; set; }
        public string Data { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
