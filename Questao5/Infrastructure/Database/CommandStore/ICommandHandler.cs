namespace Questao5.Infrastructure.Database.CommandStore
{
    public interface ICommandHandler<TCommand>
    {
        Task Execute(TCommand command);
    }
}
