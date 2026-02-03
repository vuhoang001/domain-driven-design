using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Infrastructure.EventBus;

public class InMemoryEventBusClient(ILogger logger) : IEventBus
{
    public void Dispose()
    {
    }

    public async Task Publish<T>(T @event) where T : IntegrationEvent
    {
        logger.LogInformation("Publishing {Event}", @event.GetType().FullName);
        await InMemoryEventBus.Instance.Publish(@event);
    }

    public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
    {
        InMemoryEventBus.Instance.Subscribe(handler);
    }

    public void StartConsuming()
    {
    }
}