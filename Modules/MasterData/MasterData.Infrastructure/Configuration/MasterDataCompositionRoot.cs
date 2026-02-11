using Autofac;

namespace MasterData.Infrastructure.Configuration;

internal static class MasterDataCompositionRoot
{
    private static IContainer? _container;

    internal static void SetContainer(IContainer container)
    {
        _container = container;
    }

    internal static ILifetimeScope BeginLifetimeScope()
    {
        return _container!.BeginLifetimeScope();
    }
}