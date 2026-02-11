using Autofac;
using BuildingBlocks.Application.Configuration.Commands;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using BuildingBlocks.Infrastructure.DomainEventsDispatching.Decorators;
using MediatR;

namespace BuildingBlocks.Infrastructure.Configuration;

public class BaseProcessingModule(bool enableOutbox) : Module
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
            typeof(ICommandHandler<>));

        builder.RegisterGenericDecorator(
            typeof(UnitOfWorkCommandHandlerWithResultDecorator<,>),
            typeof(ICommandHandler<,>));

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

        builder.RegisterDecorator<OutboxDomainEventsDispatcherDecorator, IDomainEventsDispatcher>();

        RegisterAdditionalDecorators(builder);
    }

    // Template methods - module implement riêng
    protected virtual void RegisterModuleSpecificServices(ContainerBuilder builder)
    {
    }

    protected virtual void RegisterAdditionalDecorators(ContainerBuilder builder)
    {
        // Default: không có decorator thêm
    }
}