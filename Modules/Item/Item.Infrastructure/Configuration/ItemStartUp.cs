using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure.Email;
using BuildingBlocks.Infrastructure.EventBus;
using Item.Infrastructure.Configuration.DataAccess;
using Item.Infrastructure.Configuration.Logging;
using Item.Infrastructure.Configuration.Mediation;
using Item.Infrastructure.Configuration.Processing;
using Serilog;
using Serilog.Extensions.Logging;

namespace Item.Infrastructure.Configuration;

public class ItemStartUp
{
    private static IContainer _container;

    public static void Initialize(
        string connectionString,
        IExecutionContextAccessor executionContextAccessor,
        ILogger logger,
        EmailConfiguration emailConfiguration,
        IEventBus? eventBus,
        long? internalProcessingPoolingInterval = null)
    {
        var moduleLogger = logger.ForContext("Module", "Administration");

        ConfigurationCompositionRoot(connectionString, executionContextAccessor, moduleLogger, emailConfiguration,
                                     eventBus);
    }

    private static void ConfigurationCompositionRoot(
        string connectionString,
        IExecutionContextAccessor executionContextAccessor,
        ILogger logger,
        EmailConfiguration emailConfiguration,
        IEventBus? eventBus
    )
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "Item")));

        var loggerFactory = new SerilogLoggerFactory(logger);
        containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
        containerBuilder.RegisterModule(new MediatorModule());
        containerBuilder.RegisterModule(new ProcessingModule());

        var container = containerBuilder.Build();
        ItemCompositionRoot.SetContainer(container);
    }
}