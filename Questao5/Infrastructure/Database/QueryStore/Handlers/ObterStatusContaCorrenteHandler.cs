using Dapper;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore.Handlers
{
    public class ObterStatusContaCorrenteHandler
        : IQueryHandler<ObterStatusContaCorrente, ObterStatusContaCorrenteResponse>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public ObterStatusContaCorrenteHandler(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<ObterStatusContaCorrenteResponse> Execute(ObterStatusContaCorrente query)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var sql = @"SELECT 1 AS Existe, ativo as Ativa FROM contacorrente WHERE idcontacorrente = @ContaCorrenteId";

            var result = await connection.QueryFirstOrDefaultAsync<ObterStatusContaCorrenteResponse>(sql, new
            {
                ContaCorrenteId = query.ContaCorrenteId.ToString().ToUpper()
            });
            return result ?? new ObterStatusContaCorrenteResponse();
        }
    }
}
