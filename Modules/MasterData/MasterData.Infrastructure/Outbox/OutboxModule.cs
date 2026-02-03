using Autofac;
using BuildingBlocks.Application.Events;
using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using Item.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Item.Infrastructure.Outbox;

public class OutboxModule(BiDictionary<string, Type> domainNotificationsMap) : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<OutboxAccessor>()
            .As<IOutbox>()
            .FindConstructorsWith(new AllConstructorFinder())
            .InstancePerLifetimeScope();

        CheckMappings();

        builder.RegisterType<DomainNotificationMapper>()
            .As<IDomainNotificationMapper>()
            .FindConstructorsWith(new AllConstructorFinder())
            .WithParameter("domainNotificationsMap", domainNotificationsMap)
            .SingleInstance();
    }

    private void CheckMappings()
    {
        var domainEventNotifications = Assemblies.Application
            .GetTypes()
            .Where(x => x.GetInterfaces().Contains(typeof(IDomainEventNotification)))
            .ToList();

        List<Type> notMappedNotifications = [];
        foreach (var domainEventNotification in domainEventNotifications)
        {
            domainNotificationsMap.TryGetBySecond(domainEventNotification, out var name);
            if (name is null)
            {
                notMappedNotifications.Add(domainEventNotification);
            }
        }

        if (notMappedNotifications.Any())
        {
            throw new ApplicationException(
                $"Domain Event Notifications {notMappedNotifications.Select(x => x.FullName).Aggregate((x, y) => x + "," + y)} not mapped");
        }
    }
}