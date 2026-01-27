namespace BuildingBlocks.Domain;

public class DomainEventBase : IDomainEvent
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }

    public DomainEventBase(Guid id, DateTime occurredOn)
    {
        Id         = id;
        OccurredOn = DateTime.UtcNow;
    }
}