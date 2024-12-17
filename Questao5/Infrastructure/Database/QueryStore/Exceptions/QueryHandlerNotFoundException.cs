namespace Questao5.Infrastructure.Database.QueryStore.Exceptions
{
    public class QueryHandlerNotFoundException : Exception
    {
        public QueryHandlerNotFoundException(string message) : base(message) {}
    }
}
