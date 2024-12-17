using System.Data;

namespace Questao5.Infrastructure.Sqlite
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
