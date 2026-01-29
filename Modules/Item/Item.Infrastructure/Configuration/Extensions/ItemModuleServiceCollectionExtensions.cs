using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure.Email;
using BuildingBlocks.Infrastructure.EventBus;
using Item.Application.Configuration.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Item.Infrastructure.Configuration.Extensions;

public static class ItemModuleServiceCollectionExtensions
{
    public static IServiceCollection AddItemModule(
        this IServiceCollection services,
        IConfiguration configuration,
        ILogger logger)
    {
        var connectionString = configuration.GetConnectionString("ItemDatabase")
                               ?? throw new InvalidOperationException("ItemDatabase connection string is not configured");

        // Register IItemModule
        services.AddSingleton<IItemModule, ItemModule>();

        // Initialize Item Module
        var executionContextAccessor = new ExecutionContextAccessor();
        
        var emailConfiguration = new EmailConfiguration(
            fromEmail: configuration["Email:FromEmail"] ?? "noreply@ddd.com"
        );

        var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog(logger));
        var eventBus = new InMemoryEventBusClient(loggerFactory.CreateLogger<InMemoryEventBusClient>());

        ItemStartUp.Initialize(
            connectionString,
            executionContextAccessor,
            logger,
            emailConfiguration,
            eventBus
        );

        return services;
    }
}

// Simple ExecutionContextAccessor implementation
internal class ExecutionContextAccessor : IExecutionContextAccessor
{
    public Guid UserId => Guid.Empty;
    public Guid CorrelationId => Guid.NewGuid();
    public bool IsAvailable => true;
}
