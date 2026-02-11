using System;
using Autofac;
using BuildingBlocks.Infrastructure.Configuration;
using JetBrains.Annotations;
using Xunit;

namespace BuildingBlocks.Infrastructure.Tests.Configuration;

[TestSubject(typeof(BaseProcessingModule))]
public class BaseProcessingModuleTest
{
    [Fact]
    public void load_should_register_validation_command_handler_decorator()
    {
        var builder = new ContainerBuilder();
        var module  = new BaseProcessingModule();
        builder.RegisterModule(module);
        var container = builder.Build();

        Console.WriteLine(container);
    }
}