using Autofac;
using Item.Application.Contracts;
using MediatR;


namespace Item.Infrastructure.Configuration.Processing;

/// <summary>
/// CommandsExecutor thực thi các commands đã được lưu trong DB(outbox table).
/// Chạy bởi background job để xử lý async commands.
/// Dảm bảo commands được thực thi theo thứ tự và không bị mất (eventual consistency).
/// </summary>
internal static class CommandsExecutor
{
    internal static async Task Execute(ICommand command)
    {
        await using var scope    = ItemCompositionRoot.BeginLifetimeScope();
        var             mediator = scope.Resolve<IMediator>();
        await mediator.Send(command);
    }

    internal static async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        await using var scope    = ItemCompositionRoot.BeginLifetimeScope();
        var             mediator = scope.Resolve<IMediator>();
        return await mediator.Send(command);
    }
}