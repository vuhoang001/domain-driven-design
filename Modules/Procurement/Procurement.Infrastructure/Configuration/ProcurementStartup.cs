using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure.Email;
using BuildingBlocks.Infrastructure.EventBus;
using Serilog;
using IContainer = System.ComponentModel.IContainer;

namespace Procurement.Infrastructure.Configuration;

public class ProcurementStartup
{
    private static IContainer _container;

    public static void Initialize(string connectionString, IExecutionContextAccessor executionContextAccessor,
        ILogger logger, EmailConfiguration emailConfiguration, IEventBus eventBus, bool runQuartz = true,
        long? internalProcessingPoolingInterval = null)
    {
        var moduleLogger = logger.ForContext("Module", "Procurement");
    }

    public static void Stop()
    {
        
    }
    private static void ConfigureCompositionRoot(
        string connectionString,
        IExecutionContextAccessor executionContextAccessor,
        ILogger logger,
        EmailConfiguration emailConfiguration,
        IEventBus eventBus,
        bool runQuartz)
    {
        var containerBuilder = new ContainerBuilder();
        
    }
}