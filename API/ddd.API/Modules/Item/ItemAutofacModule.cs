using Autofac;
using Item.Application.Configuration.Commands;
using Item.Infrastructure;

namespace ddd.API.Modules.Item;

internal class ItemAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ItemModule>()
            .As<IItemModule>()
            .InstancePerLifetimeScope();
    }
}