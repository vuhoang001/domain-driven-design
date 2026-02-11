using System;
using Autofac;
using BuildingBlocks.Infrastructure.Configuration;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Xunit;

namespace BuildingBlocks.Infrastructure.Tests.Configuration;

[TestSubject(typeof(BaseLoggingModule))]
public class BaseLoggingModuleTest
{
    [Fact]
    public void should_register_base_logging_module()
    {
        var builder = new ContainerBuilder();
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        var module = new BaseLoggingModule(logger);
        builder.RegisterModule(module);

        var container = builder.Build();

        var loggerFactory = container.Resolve<ILoggerFactory>();

        Assert.NotNull(loggerFactory);
        Assert.IsType<Serilog.Extensions.Logging.SerilogLoggerFactory>(loggerFactory);
    }
}