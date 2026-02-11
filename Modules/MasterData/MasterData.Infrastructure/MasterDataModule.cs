using Autofac;
using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Application.Configuration.Queries;
using MasterData.Application.Configuration.Command;
using MasterData.Infrastructure.Configuration;
using MediatR;

namespace MasterData.Infrastructure;

public class MasterDataModule : IMasterDataModule
{
    public async Task<TResult> ExecuteCommandAsync<TResult>(ICommand<TResult> command)
    {
        return await CommandsExecutor.Execute(command);
    }

    public async Task ExecuteCommandAsync(ICommand command)
    {
        await CommandsExecutor.Execute(command);
    }

    public async Task<TResult> ExecuteQueryAsync<TResult>(IQuery<TResult> query)
    {
        await using var scope    = MasterDataCompositionRoot.BeginLifetimeScope();
        var             mediator = scope.Resolve<IMediator>();
        return await mediator.Send(query);
    }
}