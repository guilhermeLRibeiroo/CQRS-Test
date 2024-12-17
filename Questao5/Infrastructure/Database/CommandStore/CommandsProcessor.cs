using Questao5.Infrastructure.Database.CommandStore.Exceptions;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class CommandsProcessor : ICommandsProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandsProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Process<TCommand>(TCommand command)
        {
            var handler = _serviceProvider.GetService<ICommandHandler<TCommand>>();

            if (handler == null) throw new CommandHandlerNotFoundException($"Command '{command.GetType().Name}' not found.");

            return handler.Execute(command);
        }
    }

    public interface ICommandsProcessor
    {
        Task Process<TCommand>(TCommand command);
    }
}
