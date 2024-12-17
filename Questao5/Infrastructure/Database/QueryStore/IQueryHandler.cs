namespace Questao5.Infrastructure.Database.QueryStore
{
    public interface IQueryHandler<TQuery, TResult>
    {
        abstract Task<TResult> Execute(TQuery query);
    }
}
