using Dapper;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Handlers
{
    public class CriarMovimentoHandler
        : ICommandHandler<CriarMovimento>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public CriarMovimentoHandler(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task Execute(CriarMovimento command)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"
            BEGIN TRANSACTION;

            INSERT INTO movimento
            (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
            VALUES
            (@MovimentoId, @ContaCorrenteId, @Data, @Tipo, @Valor);

            INSERT INTO idempotencia
            (chave_idempotencia, requisicao, resultado)
            VALUES
            (@IdempotenciaId, @Requisicao, @Resultado);
            
            COMMIT;";

            await connection.ExecuteAsync(sql, new
            {
                IdempotenciaId = command.IdempotenciaId.ToString().ToUpper(),
                Requisicao = command.ToString(),
                Resultado = command.MovimentoId,

                MovimentoId = command.MovimentoId.ToString().ToUpper(),
                ContaCorrenteId = command.ContaCorrenteId.ToString().ToUpper(),
                command.Data,
                command.Tipo,
                command.Valor
            });

        }
    }
}
