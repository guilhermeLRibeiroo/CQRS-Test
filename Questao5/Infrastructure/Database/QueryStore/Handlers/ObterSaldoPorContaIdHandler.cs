using Dapper;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.QueryStore.Handlers
{
    public class ObterSaldoPorContaIdHandler : IQueryHandler<ObterSaldoPorContaId, ObterSaldoPorContaIdResponse>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        public ObterSaldoPorContaIdHandler(IDbConnectionFactory connectionFactory)
        {
            _dbConnectionFactory = connectionFactory;
        }

        public async Task<ObterSaldoPorContaIdResponse> Execute(ObterSaldoPorContaId query)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            var sql = @"
            SELECT 
	            c.nome AS Titular,
	            c.numero AS Numero,
	            SUM(CASE
                WHEN m.tipomovimento = 'C' THEN m.valor
                WHEN m.tipomovimento = 'D' THEN -m.valor
                ELSE 0
                END) AS Saldo
            FROM 
	            contacorrente c
            FULL OUTER JOIN movimento m ON c.idcontacorrente = m.idcontacorrente
            WHERE c.idcontacorrente = @ContaCorrenteId";

            return await connection.QuerySingleOrDefaultAsync<ObterSaldoPorContaIdResponse>(sql, new
            {
                ContaCorrenteId = query.ContaCorrenteId.ToString().ToUpper()
            });
        }
    }
}
