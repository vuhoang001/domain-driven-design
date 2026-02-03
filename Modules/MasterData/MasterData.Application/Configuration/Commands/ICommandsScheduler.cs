using MasterData.Application.Configuration.Contracts;
using MasterData.Application.Contracts;

namespace MasterData.Application.Configuration.Commands;

public interface ICommandsScheduler
{
    Task EnqueueAsync(ICommand command);
    Task EnqueueAsync<T>(ICommand<T> command);
}