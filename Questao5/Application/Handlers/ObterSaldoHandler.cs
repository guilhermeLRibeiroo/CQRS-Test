using MediatR;
using Questao5.Application.Exceptions;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Application.Handlers
{
    public class ObterSaldoHandler
        : IRequestHandler<ObterSaldoRequest, ObterSaldoResponse>
    {
        private readonly IQueriesProcessor _queriesProcessor;
        public ObterSaldoHandler(IQueriesProcessor queriesProcessor)
        {
            _queriesProcessor = queriesProcessor;
        }

        public async Task<ObterSaldoResponse> Handle(ObterSaldoRequest request, CancellationToken cancellationToken)
        {
            var conta =
                await _queriesProcessor.Process<ObterStatusContaCorrente, ObterStatusContaCorrenteResponse>(new ObterStatusContaCorrente(request.ContaCorrenteId));

            if (!conta.Existe)
                throw new InvalidAccountException("Apenas contas correntes cadastradas podem consultar o saldo.");

            if (!conta.Ativa)
                throw new InactiveAccountException("Apenas contas correntes ativas podem consultar o saldo.");

            var queryResult = await _queriesProcessor
                .Process<ObterSaldoPorContaId, ObterSaldoPorContaIdResponse>(new ObterSaldoPorContaId(request.ContaCorrenteId));

            return new ObterSaldoResponse
            {
                Numero = queryResult.Numero,
                Titular = queryResult.Titular,
                Saldo = queryResult.Saldo
            };
        }
    }
}
