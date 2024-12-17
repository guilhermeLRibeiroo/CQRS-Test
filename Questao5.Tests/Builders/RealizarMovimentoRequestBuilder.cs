using Questao5.Application.Commands.Requests;

namespace Questao5.Tests.Builders
{
    public class RealizarMovimentoRequestBuilder
    {
        private RealizarMovimentoRequest _request;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public RealizarMovimentoRequestBuilder()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            this.Reset();
        }

        public void Reset()
        {
            _request = new RealizarMovimentoRequest();
        }

        public RealizarMovimentoRequestBuilder WithRequisicaoId(Guid requisicaoId)
        {
            _request.RequisicaoId = requisicaoId;
            return this;
        }

        public RealizarMovimentoRequestBuilder WithContaCorrenteId(Guid contaCorrenteId)
        {
            _request.ContaCorrenteId = contaCorrenteId;
            return this;
        }

        public RealizarMovimentoRequestBuilder WithValor(decimal valor)
        {
            _request.Valor = valor;
            return this;
        }

        public RealizarMovimentoRequestBuilder WithTipo(string tipo)
        {
            _request.Tipo = tipo;
            return this;
        }

        public RealizarMovimentoRequest Build()
        {
            var result = _request;
            Reset();
            return result;
        }
    }
}
