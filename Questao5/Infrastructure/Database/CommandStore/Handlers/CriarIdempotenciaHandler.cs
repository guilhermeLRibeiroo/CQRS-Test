using Dapper;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.CommandStore.Handlers
{
    public class CriarIdempotenciaHandler
        : ICommandHandler<CriarIdempotencia>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public CriarIdempotenciaHandler(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task Execute(CriarIdempotencia command)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sqlInsert = @"INSERT INTO idempotencia 
            (chave_idempotencia, requisicao, resultado)
            VALUES
            (@Id, @Requisicao, @Resultado);";

            await connection.ExecuteAsync(sqlInsert, new
            {
                Id = command.Id.ToString().ToUpper(),
                command.Requisicao,
                command.Resultado,
            });
        }
    }
}
