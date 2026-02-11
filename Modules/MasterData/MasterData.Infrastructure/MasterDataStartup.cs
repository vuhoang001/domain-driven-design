using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Application.Events;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.EventBus;
using MasterData.Infrastructure.Configuration;
using MasterData.Infrastructure.Configuration.DataAccess;
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

        var configBuilder = new ModuleConfigurationBuilder(
            moduleName: "MasterData",
            connectionString: connectionString,
            logger: moduleLogger,
            executionContextAccessor: executionContextAccessor,
            eventsBus: eventsBus ?? new InMemoryEventBusClient(moduleLogger)
        );

        configBuilder.RegisterCommonModules();

        var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(moduleLogger);


        configBuilder
            .RegisterCustomModule(new MasterDataDataAccessModule(connectionString, loggerFactory))
            .RegisterCustomModule(new MasterDataProcessingModule());

        _container = configBuilder.Build();

        MasterDataCompositionRoot.SetContainer(_container);

        moduleLogger.Information("MasterData module initialized successfully");
    }

    public static void Shutdown()
    {
        _container?.Dispose();
    }
}