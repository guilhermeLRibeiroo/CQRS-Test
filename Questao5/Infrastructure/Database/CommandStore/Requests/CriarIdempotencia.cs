namespace Questao5.Infrastructure.Database.CommandStore.Requests
{
    public class CriarIdempotencia
    {
        public string Id { get; set; }
        public string Requisicao { get; set; }
        public string Resultado { get; set; }
    }
}
