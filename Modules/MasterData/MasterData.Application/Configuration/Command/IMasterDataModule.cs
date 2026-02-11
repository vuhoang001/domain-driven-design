using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Application.Configuration.Queries;

namespace MasterData.Application.Configuration.Command;

public interface IMasterDataModule
{
    Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command);

    Task ExecuteCommandAsync(ICommand command);

    Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query);
}