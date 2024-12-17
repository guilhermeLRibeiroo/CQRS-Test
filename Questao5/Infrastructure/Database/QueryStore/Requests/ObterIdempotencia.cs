namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class ObterIdempotencia
    {
        public Guid Id { get; set; }

        public ObterIdempotencia(Guid id)
        {
            this.Id = id;
        }
    }
}
