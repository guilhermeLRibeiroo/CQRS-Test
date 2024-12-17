using ServiceStack.OrmLite;
using System.Data;

namespace Questao5.Tests.Common
{
    public class InMemoryDatabase
    {
        private readonly OrmLiteConnectionFactory dbFactory = new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider);

        public IDbConnection OpenConnection() => this.dbFactory.OpenDbConnection();

        public void Insert<T>(IEnumerable<T> items)
        {
            using (var db = this.OpenConnection())
            {
                db.CreateTableIfNotExists<T>();
                foreach (var item in items)
                {
                    db.Insert(item);
                }
            }
        }

        public void Delete<T>(IEnumerable<T> items)
        {
            using var db = this.OpenConnection();
            foreach (var item in items)
            {
                db.Delete(item);
            }
        }

        public void CreateTableIfNotExists<T>()
        {
            using var conn = this.OpenConnection();
            conn.CreateTableIfNotExists<T>();
        }
    }
}
