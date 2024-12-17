namespace Questao5.Infrastructure.Database.CommandStore.Exceptions
{
    public class CommandHandlerNotFoundException : Exception
    {
        public CommandHandlerNotFoundException(string message) : base(message) {}
    }
}
