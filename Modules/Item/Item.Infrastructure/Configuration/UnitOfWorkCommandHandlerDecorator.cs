using BuildingBlocks.Infrastructure;
using Item.Application.Configuration.Commands;
using Item.Application.Configuration.Contracts;
using Item.Application.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Item.Infrastructure.Configuration;

internal class UnitOfWorkCommandHandlerDecorator<T>(
    ICommandHandler<T> decorator,
    IUnitOfWork unitOfWork,
    ItemContext itemContext) : ICommandHandler<T> where T : ICommand
{
    public async Task Handle(T request, CancellationToken cancellationToken)
    {
        await decorator.Handle(request, cancellationToken);

        if (request is InternalCommandBase)
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
    }
}