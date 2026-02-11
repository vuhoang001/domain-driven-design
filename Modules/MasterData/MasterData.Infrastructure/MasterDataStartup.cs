using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.Configuration.Components.DataAccess;
using BuildingBlocks.Infrastructure.Configuration.Components.Logging;
using BuildingBlocks.Infrastructure.Configuration.Components.Mediator;
using BuildingBlocks.Infrastructure.Configuration.Components.Proccessing;
using BuildingBlocks.Infrastructure.EventBus;
using MasterData.Application.Configuration.Command;
using MasterData.Application.Features.SampleFeature;
using MasterData.Domain.Entities.Item.Events;
using MasterData.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace MasterData.Infrastructure;

public class MasterDataStartup
{
    private static IContainer? _container;

    public static void Initialize(
        string connectionString,
        IExecutionContextAccessor executionContextAccessor,
        ILogger logger,
        IEventsBus? eventsBus = null)
    {
        var moduleLogger = logger.ForContext("Module", "MasterData");

        var builder = new ModuleConfigurationBuilder()
            .AddComponent(new DataAccessComponent<MasterDataContext>(
                              new DataAccessOptions
                              {
                                  ConnectionString       = connectionString,
                                  EnableSensitiveLogging = true,
                                  ConfigureProvider      = options => { options.UseSqlServer(connectionString); }
                              }))
            .AddComponent(new LoggingComponent(new LoggingOptions
            {
                Logger = logger
            }))
            .AddComponent(new MediatorComponent(typeof(IMasterDataModule).Assembly))
            .AddComponent(new ProcessingComponent(enableOutbox: true));
        var domainNotificationsMap = new BiDictionary<string, Type>();
        domainNotificationsMap.Add(
            nameof(CreateItemNotification),
            typeof(CreateItemNotification));
        builder.AddComponent(new OutboxModule(domainNotificationsMap));


        _container = builder.Build();

        MasterDataCompositionRoot.SetContainer(_container);

        moduleLogger.Information("MasterData module initialized successfully");
    }

    public static void Shutdown()
    {
        _container?.Dispose();
    }
}