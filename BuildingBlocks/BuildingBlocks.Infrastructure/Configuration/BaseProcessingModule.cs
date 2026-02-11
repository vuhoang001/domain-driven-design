using Autofac;
using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using BuildingBlocks.Infrastructure.DomainEventsDispatching.Decorators;
using MediatR;

namespace BuildingBlocks.Infrastructure.Configuration;

public class DefaultProcessingModule :
    BaseProcessingModule
{
    protected override void RegisterModuleSpecificServices(ContainerBuilder builder)
    {
        throw new NotImplementedException();
    }
}

public abstract class BaseProcessingModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        RegisterCommonServices(builder);
        RegisterDecorators(builder);

        RegisterModuleSpecificServices(builder);
    }

    protected virtual void RegisterCommonServices(ContainerBuilder builder)
    {
        builder.RegisterType<DomainEventsDispatcher>()
            .As<IDomainEventsDispatcher>()
            .InstancePerLifetimeScope();
    }

    protected virtual void RegisterDecorators(ContainerBuilder builder)
    {
        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerDecorator<>),
            typeof(IRequestHandler<>));

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
            typeof(IRequestHandler<,>));

        builder.RegisterGenericDecorator(
            typeof(ValidationCommandHandlerDecorator<>),
            typeof(ICommandHandler<>));

        builder.RegisterGenericDecorator(
            typeof(ValidationCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerDecorator<>),
            typeof(IRequestHandler<>));

        builder.RegisterGenericDecorator(
            typeof(LoggingCommandHandlerWithResultDecorator<,>),
            typeof(IRequestHandler<,>));

        RegisterAdditionalDecorators(builder);
    }

    // Template methods - module implement riêng
    protected abstract void RegisterModuleSpecificServices(ContainerBuilder builder);

    protected virtual void RegisterAdditionalDecorators(ContainerBuilder builder)
    {
        // Default: không có decorator thêm
    }
}