using Autofac;
using Item.Application.Configuration.Commands;
using Item.Application.Configuration.Contracts;
using Item.Application.Contracts;
using Item.Infrastructure.Configuration;
using Item.Infrastructure.Configuration.Processing;
using MediatR;

namespace Item.Infrastructure;

public class ItemModule : IItemModule
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
        await using var scope    = ItemCompositionRoot.BeginLifetimeScope();
        var             mediator = scope.Resolve<IMediator>();
        return await mediator.Send(query);
    }
}