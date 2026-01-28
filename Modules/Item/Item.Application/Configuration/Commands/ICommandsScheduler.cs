using Item.Application.Configuration.Contracts;
using Item.Application.Contracts;

namespace Item.Application.Configuration.Commands;

public interface ICommandsScheduler
{
    Task EnqueueAsync(ICommand command);
    Task EnqueueAsync<T>(ICommand<T> command);
}