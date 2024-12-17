namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class ObterSaldoPorContaId
    {
        public Guid ContaCorrenteId { get; set; }

        public ObterSaldoPorContaId(Guid contaCorrenteId)
        {
            this.ContaCorrenteId = contaCorrenteId;
        }
    }
}
