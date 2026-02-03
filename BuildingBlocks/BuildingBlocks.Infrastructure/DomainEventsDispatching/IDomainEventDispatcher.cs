namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

/// <summary>
/// interface dùng để phân phối các sự kiện miền (domain events) trong hệ thống từ entities.
/// </summary>
public interface IDomainEventDispatcher
{
   Task DispatchEventAsync();
}