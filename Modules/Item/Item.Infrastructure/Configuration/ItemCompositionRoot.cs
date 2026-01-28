using Autofac;

namespace Item.Infrastructure.Configuration;

internal static class ItemCompositionRoot
{
    private static IContainer _container;

    internal static void SetContainer(IContainer container)
    {
        _container = container;
    }

    internal static ILifetimeScope BeginLifetimeScope()
    {
        return _container.BeginLifetimeScope();
    }
}