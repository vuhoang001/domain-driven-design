namespace BuildingBlocks.Domain.SeedWork;

public abstract class AggregateId<T>(Guid value)
    where T : AggregateRoot
{
    public Guid Value { get;  } = value;
}