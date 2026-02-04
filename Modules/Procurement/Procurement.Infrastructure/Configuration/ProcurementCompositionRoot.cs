using Autofac;

namespace Procurement.Infrastructure.Configuration;

public static class ProcurementCompositionRoot
{
    private static IContainer _container = null!;

    public static void SetContainer(IContainer container)
    {
        _container = container;
    }

    public static ILifetimeScope BeginLifetimeScope()
    {
        return _container.BeginLifetimeScope();
    }
}