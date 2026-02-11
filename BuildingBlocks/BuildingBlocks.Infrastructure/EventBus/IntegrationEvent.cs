using MediatR;

namespace BuildingBlocks.Infrastructure.EventBus
{
    public abstract class IntegrationEvent(Guid id, DateTime occurredOn) : INotification
    {
        public Guid Id { get; } = id;

        public DateTime OccurredOn { get; } = occurredOn;
    }
}