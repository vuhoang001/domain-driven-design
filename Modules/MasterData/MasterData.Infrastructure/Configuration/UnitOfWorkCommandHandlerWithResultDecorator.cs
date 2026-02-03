using BuildingBlocks.Infrastructure;
using MasterData.Application.Configuration.Commands;
using MasterData.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Item.Infrastructure.Configuration;

internal class UnitOfWorkCommandHandlerWithResultDecorator<T, TResult>(
    ICommandHandler<T, TResult> decorated,
    IUnitOfWork unitOfWork,
    MasterDataContext masterDataContext) : ICommandHandler<T, TResult>
    where T : ICommand<TResult>
{
    public async Task<TResult> Handle(T request, CancellationToken cancellationToken)
    {
        var result = await decorated.Handle(request, cancellationToken);

        if (request is InternalCommandBase<TResult>)
        {
            var internalCommand =
                await masterDataContext.InternalCommands.FirstOrDefaultAsync(x => x.Id == request.Id,
                                                                       cancellationToken: cancellationToken);

            if (internalCommand is not null)
            {
                internalCommand.EnqueueDate = DateTime.UtcNow;
            }
        }

        await unitOfWork.CommitAsync(cancellationToken);
        return result;
    }
}