using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure;

public class UnitOfWork(DbContext context, IDomainEventDispatcher domainEventDispatcher) : IUnitOfWork
{
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default, Guid? internalCommandId = null)
    {
        await domainEventDispatcher.DispatchEventAsync();
        return await context.SaveChangesAsync(cancellationToken);
    }
}