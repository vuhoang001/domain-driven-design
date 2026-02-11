using Autofac;
using Serilog;

namespace BuildingBlocks.Infrastructure.Configuration;

public class BaseLoggingModule(ILogger logger) : Module
{
    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterInstance(_logger)
            .As<ILogger>()
            .SingleInstance();

        var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(_logger);
        builder.RegisterInstance(loggerFactory)
            .As<Microsoft.Extensions.Logging.ILoggerFactory>()
            .SingleInstance();
    }
}