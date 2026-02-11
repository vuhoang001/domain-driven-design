using Autofac;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.DomainEventsDispatching.Decorators;
using JetBrains.Annotations;
using MasterData.Infrastructure.Configuration;

namespace MasterData.Infrastructure.Tests;

[TestSubject(typeof(MasterDataStartup))]
public class MasterDataStartupTest
{
    private ContainerBuilder SetupContainer()
    {
        var builder = new ContainerBuilder();
        
        // Scan MasterData.Application để tìm command handlers
        // var masterDataApplicationAssembly = typeof(CreateItemCommandHandler).Assembly;
        
        // Đăng ký các module cơ sở cần thiết
        // builder.RegisterModule(new BaseMediatorModule(masterDataApplicationAssembly));
        
        // Đăng ký MasterDataProcessingModule
        builder.RegisterModule(new MasterDataProcessingModule());
        
        return builder;
    }


    [Fact]
    public void should_register_validation_command_handler_decorator()
    {
        // Arrange
        var builder = SetupContainer();
        var container = builder.Build();

        // Act
        var registrations = container.ComponentRegistry.Registrations;

        // Assert
        Assert.NotEmpty(registrations.Where(r =>
                                                r.Activator.LimitType == typeof(ValidationCommandHandlerDecorator<>)));
    }

    [Fact]
    public void should_register_logging_command_handler_decorator()
    {
        // Arrange
        var builder = SetupContainer();
        var container = builder.Build();

        // Act
        var registrations = container.ComponentRegistry.Registrations;

        // Assert
        Assert.NotEmpty(registrations.Where(r =>
                                                r.Activator.LimitType == typeof(LoggingCommandHandlerDecorator<>)));
    }

    [Fact]
    public void should_register_unit_of_work_decorator()
    {
        // Arrange
        var builder = SetupContainer();
        var container = builder.Build();

        // Act
        var registrations = container.ComponentRegistry.Registrations;

        // Assert
        Assert.NotEmpty(registrations.Where(r =>
                                                r.Activator.LimitType == typeof(UnitOfWorkCommandHandlerDecorator<>)));
    }
}


