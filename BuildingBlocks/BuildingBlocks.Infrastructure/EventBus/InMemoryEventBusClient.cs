using Serilog;

namespace BuildingBlocks.Infrastructure.EventBus
{
    public class InMemoryEventBusClient(ILogger logger) : IEventsBus
    {
        public void Dispose()
        {
        }

        public async Task Publish<T>(T @event)
            where T : IntegrationEvent
        {
            logger.Information("Publishing {Event}", @event.GetType().FullName);
            await InMemoryEventBus.Instance.Publish(@event);
        }

        public void Subscribe<T>(IIntegrationEventHandler<T> handler)
            where T : IntegrationEvent
        {
            InMemoryEventBus.Instance.Subscribe(handler);
        }

        public void StartConsuming()
        {
        }
    }
}