using BuildingBlocks.Infrastructure;
using Item.Application.Configuration.Commands;
using Item.Application.Configuration.Contracts;
using Item.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Item.Infrastructure.Configuration;

internal class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorator,
    IUnitOfWork unitOfWork,
    ItemContext itemContext) : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    public async Task<TResult> Handle(T request, CancellationToken cancellationToken)
    {
        var result = await decorator.Handle(request, cancellationToken);

        if (request is InternalCommandBase<TResult>)
        {
            var internalCommand =
                await itemContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == request.Id,
                                                                       cancellationToken: cancellationToken);

            if (internalCommand is not null)
            {
                internalCommand.ProcessedDate = DateTime.UtcNow;
            }
        }

        await unitOfWork.CommitAsync(cancellationToken);
        return result;
    }
}