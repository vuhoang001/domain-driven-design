namespace BuildingBlocks.Infrastructure.EventBus;

/// <summary>
/// Đây là constract:
///  + Publish: Đẩy một integrationEvent lên bus
///  + Subscribe: đăng ký một handler để xử lý một loại integrationEvent cụ thể
///  + StartConsuming: dùng cho các implement có hàng đợi/background consumer (RabbitMQ, Kafka, ...). Trong Inmemory thì nó để trống.
/// Ý tưởng:
///  + InMemoryEventBusClient:
///  + EventBusClient
/// => Code ở các module chỉ biết đến IEventBus, không quan tâm bên dưới dùng in-memory hay là RabbitMQ, Kafka ...
/// </summary>
public interface IEventBus : IDisposable
{
    Task Publish<T>(T @event) where T : IntegrationEvent;

    void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent;
    
    void StartConsuming();
}