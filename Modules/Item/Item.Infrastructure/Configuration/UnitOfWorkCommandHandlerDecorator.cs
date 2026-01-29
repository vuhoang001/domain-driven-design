using BuildingBlocks.Infrastructure;
using Item.Application.Configuration.Commands;
using Item.Application.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Item.Infrastructure.Configuration;

internal class UnitOfWorkCommandHandlerDecorator<T>(
    IRequestHandler<T> decorated,
    IUnitOfWork unitOfWork,
    ItemContext itemContext) : ICommandHandler<T> where T : ICommand
{
    public async Task Handle(T request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine($"🟢 DECORATOR START (No Result): {typeof(T).Name}");
            
            await decorated.Handle(request, cancellationToken);

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
            Console.WriteLine($"✅ DECORATOR END (No Result): {typeof(T).Name} - Committed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ DECORATOR ERROR (No Result): {typeof(T).Name} - {ex.Message}");
            throw;
        }
    }
}