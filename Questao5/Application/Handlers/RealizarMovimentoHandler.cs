using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Exceptions;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Application.Handlers
{
    public class RealizarMovimentoHandler
        : IRequestHandler<RealizarMovimentoRequest, RealizarMovimentoResponse>
    {
        private readonly IQueriesProcessor _queriesProcessor;
        private readonly ICommandsProcessor _commandsProcessor;

        public RealizarMovimentoHandler(IQueriesProcessor queriesProcessor,
            ICommandsProcessor commandsProcessor)
        {
            _queriesProcessor = queriesProcessor;
            _commandsProcessor = commandsProcessor;
        }

        public async Task<RealizarMovimentoResponse> Handle(RealizarMovimentoRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Tipo)
                || (request.Tipo.ToUpper()[0] != 'C' && request.Tipo.ToUpper()[0] != 'D'))
                throw new InvalidTypeException("Apenas os tipos 'débito' (D) ou 'crédito' (C) podem ser aceitos.");


            if (request.Valor <= 0)
                throw new InvalidValueException("Apenas valores positivos podem ser recebidos.");

            var conta =
                await _queriesProcessor.Process<ObterStatusContaCorrente, ObterStatusContaCorrenteResponse>(new ObterStatusContaCorrente(request.ContaCorrenteId));

            if (!conta.Existe)
                throw new InvalidAccountException("Apenas contas correntes cadastradas podem receber movimentação.");

            if (!conta.Ativa)
                throw new InactiveAccountException("Apenas contas correntes ativas podem receber movimentação.");

            var idempotencia = await _queriesProcessor.Process<ObterIdempotencia, ObterIdempotenciaResponse>(new ObterIdempotencia(request.RequisicaoId));

            if(idempotencia != null)
            {
                // Se tivesse queue criaria a idempotencia e colocaria na queue
                // a criação do movimento e caso tentar fazer novamente o movimento
                // retornaria Status Code 202 dizendo que a requisição está sendo processada

                if (string.IsNullOrWhiteSpace(idempotencia.Resultado))
                    throw new PendingIdempotency("Sua requisição está sendo processada.");

                return new RealizarMovimentoResponse
                {
                    MovimentoId = idempotencia.Resultado
                };
            }

            var novoMovimento = new CriarMovimento
            {
                MovimentoId = Guid.NewGuid(),
                IdempotenciaId = request.RequisicaoId,
                ContaCorrenteId = request.ContaCorrenteId,
                Data = DateTime.UtcNow.ToString("dd/MM/yyyy"),
                Tipo = request.Tipo,
                Valor = request.Valor,
            };

            await _commandsProcessor.Process(novoMovimento);

            return new RealizarMovimentoResponse
            {
                MovimentoId = novoMovimento.MovimentoId.ToString().ToUpper()
            };
        }
    }
}
