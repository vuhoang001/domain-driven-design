using MediatR;

namespace BuildingBlocks.Infrastructure.EventBus;

/// <summary>
/// Gói sự kiện tích hợp cơ bản.
/// </summary>
/// <param name="id"></param>
/// <param name="occurredOn"></param>
public abstract class IntegrationEvent(Guid id, DateTime occurredOn) : INotification
{
    public Guid Id { get; } = id;

    public DateTime OccurredOn { get; } = occurredOn;
}