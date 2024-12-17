using Dapper;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore.Handlers
{
    public class ObterIdempotenciaHandler
        : IQueryHandler<ObterIdempotencia, ObterIdempotenciaResponse>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public ObterIdempotenciaHandler(IDbConnectionFactory connectionFactory)
        {
            _dbConnectionFactory = connectionFactory;
        }
        public async Task<ObterIdempotenciaResponse> Execute(ObterIdempotencia query)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var sql = @"SELECT
            chave_idempotencia AS Id,
            requisicao AS Requisicao,
            resultado
            FROM idempotencia
            WHERE chave_idempotencia = @Id;";

            return await connection.QuerySingleOrDefaultAsync<ObterIdempotenciaResponse>(sql, new
            {
                Id = query.Id.ToString().ToUpper()
            });
        }
    }
}
