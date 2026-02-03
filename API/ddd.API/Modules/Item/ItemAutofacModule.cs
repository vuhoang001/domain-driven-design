using Autofac;
using Item.Infrastructure;
using MasterData.Application.Configuration.Commands;

namespace ddd.API.Modules.Item;

internal class ItemAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MasterDataModule>()
            .As<IMasterDataModule>()
            .InstancePerLifetimeScope();
    }
}