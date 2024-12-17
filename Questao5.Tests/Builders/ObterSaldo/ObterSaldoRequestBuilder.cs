using Questao5.Application.Queries.Requests;

namespace Questao5.Tests.Builders.ObterSaldo
{
    public class ObterSaldoRequestBuilder
    {
        private ObterSaldoRequest _request;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ObterSaldoRequestBuilder()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this.Reset();
        }

        public void Reset()
        {
            _request = new ObterSaldoRequest();
        }

        public ObterSaldoRequestBuilder WithContaCorrenteId(Guid contaCorrenteId)
        {
            _request.ContaCorrenteId = contaCorrenteId;
            return this;
        }

        public ObterSaldoRequest Build()
        {
            var result = _request;
            Reset();
            return result;
        }
    }
}
