namespace BuildingBlocks.Infrastructure.EventBus;

/// <summary>
/// Bất kỳ class nào muốn xử lý sự kiện phải triển khai interface này.
/// </summary>
/// <typeparam name="TIntegrationEvent"></typeparam>
public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}

public interface IIntegrationEventHandler
{
}