using Autofac;
using BuildingBlocks.Application.Events;
using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.Configuration.Components;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;

namespace MasterData.Infrastructure.Configuration;

internal class OutboxModule(BiDictionary<string, Type> domainNotificationsMap) : IModuleComponent
{
    public string Name => "Outbox";

    public void Validate()
    {
        CheckMappings();
    }

    public void Register(ContainerBuilder builder)
    {
        builder.RegisterType<OutboxAccessor>()
            .As<IOutbox>()
            .FindConstructorsWith(new AllConstructorFinder())
            .InstancePerLifetimeScope();

        builder.RegisterType<DomainNotificationsMapper>()
            .As<IDomainNotificationsMapper>()
            .WithParameter("domainNotificationsMap", domainNotificationsMap)
            .SingleInstance();
        
        builder.RegisterAssemblyTypes(Assemblies.Infrastructure)
            .Where(type => type.Name.EndsWith("Repository"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope()
            .FindConstructorsWith(new AllConstructorFinder());

    }

    private void CheckMappings()
    {
        var domainEventNotifications = Assemblies.Application
            .GetTypes()
            .Where(x => x.GetInterfaces()
                       .Any(i => i.IsGenericType &&
                                i.GetGenericTypeDefinition() == typeof(IDomainEventNotification<>)))
            .ToList();

        var notMappedNotifications = domainEventNotifications
            .Where(notification =>
                       !domainNotificationsMap.TryGetBySecond(notification, out _))
            .ToList();

        if (notMappedNotifications.Any())
        {
            throw new ApplicationException(
                $"Domain Event Notifications not mapped: " +
                string.Join(",", notMappedNotifications.Select(x => x.FullName)));
        }
    }
}