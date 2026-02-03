using BuildingBlocks.Domain;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainEventAccessor(DbContext dbContext) : IDomainEventAccessor
{
    public IReadOnlyList<IDomainEvent> GetAllDomainEvents()
    {
        var domainEvents = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Count > 0).ToList();


        return domainEvents
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();
    }

    public void ClearAllDomainEvents()
    {
        var domainEvents = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Count > 0).ToList();

        domainEvents.ForEach(x => x.Entity.ClearDomainEvents());
    }
}