using Autofac;
using Procurement.Application.Configuration.Commands;
using Procurement.Application.Contracts;

namespace Procurement.Infrastructure.Configuration;

internal static class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        await using var scope       = ProcurementCompositionRoot.BeginLifetimeScope();
        var             commandType = command.GetType();
        var             handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);

        dynamic handler = scope.Resolve(handlerType);

        await handler.Handle((dynamic)command, CancellationToken.None);
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        await using var scope       = ProcurementCompositionRoot.BeginLifetimeScope();
        var             commandType = command.GetType();
        var             handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));

        dynamic handler = scope.Resolve(handlerType);

        return await handler.Handle((dynamic)command, CancellationToken.None);
    }
}
