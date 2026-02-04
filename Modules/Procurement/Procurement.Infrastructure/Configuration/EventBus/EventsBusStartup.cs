using Autofac;
using BuildingBlocks.Infrastructure.EventBus;
using MasterData.IntegrationEvent;
using Serilog;

namespace Procurement.Infrastructure.Configuration.EventBus;

public static class EventsBusStartup
{
    public static void Initialize(ILogger logger)
    {
        SubscribeToIntegrationEvents(logger);
    }

    private static void SubscribeToIntegrationEvents(ILogger logger)
    {
        var eventBus = ProcurementCompositionRoot.BeginLifetimeScope().Resolve<IEventBus>();
        SubscribeToIntegrationEvent<CreateItemIntegrationEvent>(eventBus, logger);
    }

    private static void SubscribeToIntegrationEvent<T>(IEventBus eventBus, ILogger logger) where T : IntegrationEvent
    {
        logger.Information("Subscribe to {@IntegrationEvent}", typeof(T).FullName);
        eventBus.Subscribe(new IntegrationEventGenericHandler<T>());
    }
}