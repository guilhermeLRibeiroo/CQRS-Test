using Dapper;
using Questao5.Domain.Entities;
using Questao5.Tests.Common;
using System.Data;

namespace Questao5.Tests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        private InMemoryDatabase _database;

        public DatabaseFixture()
        {
            _database = new InMemoryDatabase();
            Setup();
        }

        private void Setup()
        {
            _database.Insert(new List<ContaCorrente>
            {
                new ContaCorrente
                {
                    IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
                    Numero = 123,
                    Nome = "Katherine Sanchez",
                    Ativo = true
                },
                new ContaCorrente
                {
                    IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
                    Numero = 456,
                    Nome = "Eva Woodward",
                    Ativo = true
                },
                new ContaCorrente
                {
                    IdContaCorrente = "382D323D-7067-ED11-8866-7D5DFA4A16C9",
                    Numero = 789,
                    Nome = "Tevin Mcconnell",
                    Ativo = true
                },
                new ContaCorrente
                {
                    IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                    Numero = 741,
                    Nome = "Ameena Lynn",
                    Ativo = false
                },
                new ContaCorrente
                {
                    IdContaCorrente = "BCDACA4A-7067-ED11-AF81-825DFA4A16C9",
                    Numero = 852,
                    Nome = "Jarrad Mckee",
                    Ativo = false
                },
                new ContaCorrente
                {
                    IdContaCorrente = "D2E02051-7067-ED11-94C0-835DFA4A16C9",
                    Numero = 963,
                    Nome = "Elisha Simons",
                    Ativo = false
                }
            });

            _database.CreateTableIfNotExists<Movimento>();
            _database.CreateTableIfNotExists<Idempotencia>();
        }

        public void Insert<T>(IEnumerable<T> list)
        {
            _database.Insert(list);
        }

        public void Delete<T>(IEnumerable<T> list)
        {
            _database.Delete(list);
        }

        public IDbConnection GetConnection()
        {
            return _database.OpenConnection();
        }

        public void Dispose()
        {
            using var db = _database.OpenConnection();
            var movimentos = db.Query<Movimento>("SELECT * FROM movimento;");
            var contas = db.Query<ContaCorrente>("SELECT * FROM contacorrente;");
            var idempotencias = db.Query<Idempotencia>("SELECT * FROM idempotencia;");

            this.Delete(contas);
            this.Delete(idempotencias);
            this.Delete(movimentos);
        }
    }
}
