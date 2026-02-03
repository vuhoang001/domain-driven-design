namespace BuildingBlocks.Infrastructure.DomainEventsDispatching;

public class DomainNotificationMapper(BiDictionary<string, Type> domainNotificationsMap) : IDomainNotificationMapper
{
    public string GetName(Type type)
    {
        return domainNotificationsMap.TryGetBySecond(type, out var name) ? name : null;
    }

    public Type GetType(string name)
    {
        return domainNotificationsMap.TryGetByFirst(name, out var type) ? type : null;
    }
}