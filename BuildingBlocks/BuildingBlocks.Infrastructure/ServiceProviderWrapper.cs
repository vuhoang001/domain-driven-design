using Autofac;

namespace BuildingBlocks.Infrastructure
{
    public class ServiceProviderWrapper(ILifetimeScope lifeTimeScope) : IServiceProvider
    {
#nullable enable
        public object? GetService(Type serviceType) => lifeTimeScope.ResolveOptional(serviceType);
    }
}
