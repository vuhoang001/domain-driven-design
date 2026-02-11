using Autofac;
using MasterData.Application.Configuration.Command;
using MasterData.Infrastructure;

namespace ddd.API.Modules.MasterData;

public class MasterDataAutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<MasterDataModule>()
            .As<IMasterDataModule>()
            .InstancePerLifetimeScope();
    }
}