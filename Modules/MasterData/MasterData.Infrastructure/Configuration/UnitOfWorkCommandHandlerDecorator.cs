using BuildingBlocks.Infrastructure;
using MasterData.Application.Configuration.Commands;
using MasterData.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Item.Infrastructure.Configuration;

internal class UnitOfWorkCommandHandlerDecorator<T>(
    ICommandHandler<T> decorated,
    IUnitOfWork unitOfWork,
    MasterDataContext masterDataContext) : ICommandHandler<T> where T : ICommand
{
    public async Task Handle(T request, CancellationToken cancellationToken)
    {
        await decorated.Handle(request, cancellationToken);

        if (request is InternalCommandBase)
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
    }
}