namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

/// <summary>
/// IDomainNotificationMapper dùng để ánh xạ giữa tên và kiểu của các thông báo miền (domain notifications) => dùng để serializa/deserialize events.
/// </summary>
public interface IDomainNotificationMapper
{
    string GetName(Type type);

    Type GetType(string name);
}