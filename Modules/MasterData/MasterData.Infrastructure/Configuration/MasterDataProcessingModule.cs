using Autofac;
using BuildingBlocks.Infrastructure.Configuration;
using BuildingBlocks.Infrastructure.DomainEventsDispatching;
using CompanyName.MyMeetings.BuildingBlocks.Application.Outbox;

namespace MasterData.Infrastructure.Configuration;

public class MasterDataProcessingModule : BaseProcessingModule
{
    protected override void RegisterModuleSpecificServices(ContainerBuilder builder)
    {
        // Register DomainEventsAccessor
        builder.RegisterType<DomainEventsAccessor>()
            .As<IDomainEventsAccessor>()
            .InstancePerLifetimeScope();

        builder.RegisterType<Outbox>()
            .As<IOutbox>()
            .InstancePerLifetimeScope();
    }


    protected override void RegisterAdditionalDecorators(ContainerBuilder builder)
    {
    }
}