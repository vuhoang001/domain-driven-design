using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure
{
    public class UnitOfWork(
        DbContext context,
        IDomainEventsDispatcher domainEventsDispatcher)
        : IUnitOfWork
    {
        public async Task<int> CommitAsync(
            CancellationToken cancellationToken = default,
            Guid? internalCommandId = null)
        {
            await domainEventsDispatcher.DispatchEventsAsync();

            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}