namespace Questao5.Infrastructure.Database.QueryStore.Requests
{
    public class ObterStatusContaCorrente
    {
        public Guid ContaCorrenteId { get; set; }

        public ObterStatusContaCorrente(Guid contaCorrenteId)
        {
            this.ContaCorrenteId = contaCorrenteId;
        }
    }
}
