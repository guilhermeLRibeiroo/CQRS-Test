using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.CommandStore.Handlers;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Database.QueryStore.Handlers;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Responses;

namespace Questao5.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCommandHandler<TCommandHandler, TCommand> (this IServiceCollection services)
            where TCommandHandler : class, ICommandHandler<TCommand>
            where TCommand : class
        {
            services.AddScoped(typeof(ICommandHandler<TCommand>), typeof(TCommandHandler));
        }

        public static void AddQueryHandler<TQueryHandler, TQuery, TResult>(this IServiceCollection services)
            where TQueryHandler : class, IQueryHandler<TQuery, TResult>
            where TQuery : class
        {
            services.AddScoped(typeof(IQueryHandler<TQuery, TResult>), typeof(TQueryHandler));
        }

        public static void AddCommandsAndQueriesDependencies(this IServiceCollection services)
        {
            services.AddScoped<ICommandsProcessor, CommandsProcessor>();
            services.AddScoped<IQueriesProcessor, QueriesProcessor>();

            services.AddQueryHandler<ObterSaldoPorContaIdHandler, ObterSaldoPorContaId, ObterSaldoPorContaIdResponse>();
            services.AddQueryHandler<ObterStatusContaCorrenteHandler, ObterStatusContaCorrente, ObterStatusContaCorrenteResponse>();
            services.AddQueryHandler<ObterIdempotenciaHandler, ObterIdempotencia, ObterIdempotenciaResponse>();

            services.AddCommandHandler<CriarMovimentoHandler, CriarMovimento>();
            services.AddCommandHandler<CriarIdempotenciaHandler, CriarIdempotencia>();
        }
    }
}
