namespace BuildingBlocks.Application.Events;

public class DomainEventNotification<T>(T domainEvent, Guid id) : IDomainEventNotification<T>
{
    public T DomainEvent { get; } = domainEvent;
    public Guid Id { get; } = id;
}