using Autofac;
using BuildingBlocks.Application.Configuration.Commands;
using MasterData.Infrastructure.Configuration;

namespace MasterData.Infrastructure;

/// <summary>
/// CommandsExecutor thực thi các commands đã được lưu trong DB(outbox table).
/// Chạy bởi background job để xử lý async commands.
/// Dảm bảo commands được thực thi theo thứ tự và không bị mất (eventual consistency).
/// </summary>
internal static class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        await using var scope = MasterDataCompositionRoot.BeginLifetimeScope();

        var     commandType = command.GetType();
        var     handlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
        dynamic handler     = scope.Resolve(handlerType);
        await handler.Handle((dynamic)command, CancellationToken.None);
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        await using var scope       = MasterDataCompositionRoot.BeginLifetimeScope();
        var             commandType = command.GetType();
        var             handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResult));

        dynamic handler = scope.Resolve(handlerType);

        return await handler.Handle((dynamic)command, CancellationToken.None);
    }
}